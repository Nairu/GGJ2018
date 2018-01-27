using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Smf;
using System;
using Melanchall.DryWetMidi.Smf.Interaction;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    public NoteSpawner spawner;

    MidiFile f;
    TempoMap tMap;
    AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        f = MidiFile.Read(Application.dataPath + "/Midi/" + "level0.mid");
        tMap = f.GetTempoMap();
        audioSource = GetComponent<AudioSource>();

        var seconds = tMap.Tempo.Values.ToArray().First().Value.MicrosecondsPerQuarterNote / 1000000f;
        Time.fixedDeltaTime = seconds;
        Debug.Log(seconds);

        notes = f.GetNotes();
        foreach (var n in notes)
        {
            //Debug.Log("Note: " + n.NoteName + " at time: " + n.Time);
        }

        if (audioSource.isPlaying)
            audioSource.Play();
        //    Debug.Log(v.NoteNumber);
    }

    IEnumerable<Melanchall.DryWetMidi.Smf.Interaction.Note> notes;

    //we start one beat ahead because we need to spawn the sprite on the previous beat
    int currentBeat = 1;
    private void FixedUpdate()
    {
        currentBeat++;
        var notesInTimestep = notes.AtTime(currentBeat * 960);
        
        if (notesInTimestep.Count() > 0)
        {
            foreach (var note in notesInTimestep)
            {
                switch (note.NoteName)
                {
                    case Melanchall.DryWetMidi.Common.NoteName.C:
                        break;
                }
                spawner.SpawnNote(ButtonEnum.A);
            }
        }
    }
    
    private void Update()
    {
        
    }
}
