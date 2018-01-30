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

    void InitialiseSong()
    {
        f = MidiFile.Read(Application.dataPath + "/Midi/" + MidiName);
        tMap = f.GetTempoMap();
        audioSource = GetComponent<AudioSource>();
        Debug.Log(Song.name);
        audioSource.clip = Song;

        var seconds = tMap.Tempo.Values.ToArray().First().Value.MicrosecondsPerQuarterNote / 1000000f;
        Time.fixedDeltaTime = seconds;

        notes = f.GetNotes();
        chords = f.GetChords();
        var notesInDifficult = notes.Count(x => x.Octave == (int)difficulty);
        spawner.NumberOfNotes = notesInDifficult;

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

    IEnumerable<Melanchall.DryWetMidi.Smf.Interaction.Note> notes;
    IEnumerable<Melanchall.DryWetMidi.Smf.Interaction.Chord> chords;

    //we start one beat ahead because we need to spawn the sprite on the previous beat
    int currentBeat = 2;
    private void FixedUpdate()
    {
        if (!PlayingSongForLevel)
            return;

        currentBeat++;
        var notesInTimestep = notes.AtTime(currentBeat * 960).Where(x => x.Octave == (int)difficulty);
        //var chordsInTimestep = chords.AtTime(currentBeat * 960).Where(x => x.Notes.Select(x => x.Octave == (int)difficulty));
        var chordsInTimestep = chords.AtTime(currentBeat * 960);
        //allNotesPlaying.Add(chordsInTimestep.Select(x => x.Notes).Select(x => x).Where(x => x))

        if (!audioSource.isPlaying)
        {
            //have we finished?
            if ((currentBeat * 960) > f.GetNotes().Max(x => x.Time))
            {
                //we're done, let the note spawner know we've finished!
                spawner.SongFinished();
            }
        }

        if (notesInTimestep.Count() > 0)
        {
            foreach (var note in notesInTimestep)
            {
                SpawnNoteForNoteName(note);
            }
        }
        else if (chordsInTimestep.Count() > 0)
        {
            foreach (var chord in chordsInTimestep)
            {
                var note = chord.Notes.FirstOrDefault(x => x.Octave == (int)difficulty);
                if (note == null || note.Equals(default(Note)))
                    continue;

                SpawnNoteForNoteName(chord.Notes.First());
            }
        }
    }

    private void SpawnNoteForNoteName(Melanchall.DryWetMidi.Smf.Interaction.Note note)
    {
        switch (note.NoteName)
        {
            case Melanchall.DryWetMidi.Common.NoteName.C:
                spawner.SpawnNote(ButtonEnum.A);
                break;
            case Melanchall.DryWetMidi.Common.NoteName.CSharp:
                spawner.SpawnNote(ButtonEnum.B);
                break;
            case Melanchall.DryWetMidi.Common.NoteName.D:
                spawner.SpawnNote(ButtonEnum.X);
                break;
            case Melanchall.DryWetMidi.Common.NoteName.DSharp:
                spawner.SpawnNote(ButtonEnum.Y);
                break;
        }
    }    
}
