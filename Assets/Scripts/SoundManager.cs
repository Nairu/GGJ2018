using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;
using System;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    MidiFile f;
    TempoMap tMap;
    AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        f = MidiFile.Read(Application.dataPath + "/Midi/" + "tetris_midi.mid");
        tMap = f.GetTempoMap();
        audioSource = GetComponent<AudioSource>();
        if (audioSource.isPlaying)
            audioSource.Play();
        //foreach (var v in f.GetNotes().AtTime(Convert.ToInt64(1)))
        //    Debug.Log(v.NoteNumber);
    }

    // Update is called once per frame
    void Update()
    {
        //audioSource.GetSpectrumData(float[], 0, FFTWindow.BlackmanHarris);
        foreach (var v in f.GetNotes().AtTime(new MetricTimeSpan(0, 0, Convert.ToInt32(audioSource.time + 1)), tMap, LengthedObjectPart.Entire))
            Debug.Log(v.NoteNumber);
    }
}
