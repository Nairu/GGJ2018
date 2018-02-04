using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Smf;
using System;
using Melanchall.DryWetMidi.Smf.Interaction;
using System.Linq;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public enum SongDifficulty
    {
        Easy=1,
        Medium=2,
        Hard=3,
        Insane=4
    }

    public NoteSpawner spawner;
    private bool PlayingSongForLevel = false;

    public SongDifficulty difficulty = SongDifficulty.Easy;
    public string MidiName;
    public AudioClip Song;

    MidiFile f;
    TempoMap tMap;
    AudioSource audioSource;
    
    IEnumerable<Melanchall.DryWetMidi.Smf.Interaction.Note> notes;
    IEnumerable<Chord> chords;

    Dictionary<SongDifficulty, Dictionary<long, Chord>> NotesInSong = new Dictionary<SongDifficulty, Dictionary<long, Chord>>();
    long songLengthMicroseconds;

    // Use this for initialization
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    int timeSigBeats = 4;
    int timeSigBars = 4;

    void InitialiseSong()
    {
        f = MidiFile.Read(Application.dataPath + "/Midi/" + MidiName);
        tMap = f.GetTempoMap();
        audioSource = GetComponent<AudioSource>(); //MAYBE WE SHOULD LOOK FOR ONE IN THE SCENE? MAKES LOGIC CLEANER
        Debug.Log(Song.name);
        audioSource.clip = Song;

        //1000000 microseconds in a second, this lets us set our fixed deltatime logic.
        if (tMap.TimeSignature.Values.Count() != 0)
        {
            timeSigBars = tMap.TimeSignature.Values.First().Value.Denominator;
            timeSigBeats = tMap.TimeSignature.Values.First().Value.Numerator;
        }
        
        var seconds = tMap.Tempo.Values.ToArray().First().Value.MicrosecondsPerQuarterNote / (1000000f * timeSigBars);
        TimeDivision t = tMap.TimeDivision;
        Debug.Log(t.ToString());
        Time.fixedDeltaTime = seconds;
        songLengthMicroseconds = (f.GetTimedEvents().LastOrDefault(e => e.Event is NoteOffEvent)?.TimeAs<MetricTimeSpan>(tMap) ?? new MetricTimeSpan()).TotalMicroseconds;
        notes = f.GetNotes();
        chords = f.GetChords();

        if (songLengthMicroseconds == 0)
        {
            songLengthMicroseconds = Math.Max(notes.Max(x => x.Time), chords.Max(x => x.Time));
        }

        for(int i = 1; i < 5; i++)
        {
            NotesInSong[(SongDifficulty)i] = new Dictionary<long, Chord>();
        }

        foreach (Melanchall.DryWetMidi.Smf.Interaction.Note note in notes)
        {
            SongDifficulty d = (SongDifficulty)note.Octave;

            var difficultDictionary = NotesInSong[d];

            if (difficultDictionary.ContainsKey(note.Time))
            {
                //check if the chord at the target time contains us:
                if (difficultDictionary[note.Time].Notes.Contains(note))
                    continue;
                else
                {
                    //if (d == difficulty)
                    //    Debug.Log("Adding note at time: " + note.Time);
                    difficultDictionary[note.Time].Notes.Add(note);
                }
            }
            else
            {
                //if (d == difficulty)
                //    Debug.Log("Adding note at time: " + note.Time);
                difficultDictionary[note.Time] = new Chord(new Melanchall.DryWetMidi.Smf.Interaction.Note[] { note });
            }
        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        //    Debug.Log(v.NoteNumber);
    }

    public void Reset()
    {
        f = null;
        tMap = null;
        notes = null;
        chords = null;
        spawner = null;
        audioSource.clip = null;
        difficulty = SongDifficulty.Easy;
        MidiName = "";

        PlayingSongForLevel = false;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (spawner == null)
        {
            if (FindObjectOfType<NoteSpawner>() != null)
            {
                spawner = FindObjectOfType<NoteSpawner>();
                InitialiseSong();
                PlayingSongForLevel = true;
            }
            else
            {
                PlayingSongForLevel = false;
            }
        }
    }

    //we start one beat ahead because we need to spawn the sprite on the previous beat
    int currentBeat = 7;
    private void FixedUpdate()
    {
        //Debug.Log("Current beat: " + currentBeat);

        if (!PlayingSongForLevel)
            return;

        currentBeat++;

        if (!NotesInSong[difficulty].ContainsKey(currentBeat * (960/4)))
        {
            //Debug.Log("No chord at this time: " + (currentBeat * (960/4)));
            return;
        }

        var chord = NotesInSong[difficulty][currentBeat * (960/4)];
        if (chord.Notes.Count() > 0)
        {
            //Debug.Log("Spawning " + chord.Notes.Count() + " notes.");
            SpawnNotesForChord(chord);
        }

        //var notesInTimestep = notes.AtTime(currentBeat * 960).Where(x => x.Octave == (int)difficulty);
        ////var chordsInTimestep = chords.AtTime(currentBeat * 960).Where(x => x.Notes.Select(x => x.Octave == (int)difficulty));
        //var chordsInTimestep = chords.AtTime(currentBeat * 960);
        ////allNotesPlaying.Add(chordsInTimestep.Select(x => x.Notes).Select(x => x).Where(x => x))

        if (!audioSource.isPlaying)
        {
            //have we finished?
            if ((currentBeat * 960) > (songLengthMicroseconds / 1000000))
            {
                //we're done, let the note spawner know we've finished!
                //spawner.SongFinished();
            }
        }

        //if (notesInTimestep.Count() > 0)
        //{
        //    foreach (var note in notesInTimestep)
        //    {
        //        SpawnNoteForNoteName(note);
        //    }
        //}
        //else if (chordsInTimestep.Count() > 0)
        //{
        //    foreach (var chord in chordsInTimestep)
        //    {
        //        var note = chord.Notes.FirstOrDefault(x => x.Octave == (int)difficulty);
        //        if (note == null || note.Equals(default(Note)))
        //            continue;

        //        SpawnNoteForNoteName(chord.Notes.First());
        //    }
        //}
    }

    private void SpawnNotesForChord(Chord chord)
    {
        List<ButtonEnum> buttons = new List<ButtonEnum>();

        foreach (Melanchall.DryWetMidi.Smf.Interaction.Note note in chord.Notes)
        {
            switch (note.NoteName)
            {
                case Melanchall.DryWetMidi.Common.NoteName.C:
                    buttons.Add(ButtonEnum.A);
                    break;
                case Melanchall.DryWetMidi.Common.NoteName.CSharp:
                    buttons.Add(ButtonEnum.B);
                    break;
                case Melanchall.DryWetMidi.Common.NoteName.D:
                    buttons.Add(ButtonEnum.X);
                    break;
                case Melanchall.DryWetMidi.Common.NoteName.DSharp:
                    buttons.Add(ButtonEnum.Y);
                    break;
            }
        }

        spawner.SpawnNote(buttons);
    }    
}
