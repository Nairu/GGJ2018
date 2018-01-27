using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Smf;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        //var x = Resources.Load<MidiFile>("");
        //var x = Resources.GetBuiltinResource<MidiFileFormat>("");
        //MidiFile.Read(Resources.GetBuiltinResource)
        MidiFile f = MidiFile.Read(Application.dataPath + "/Midi/" + "tetris_midi");
        //audioSource = GetComponent<AudioSource>();
        //if (audioSource.isPlaying)
        //    audioSource.Play();
        if (f.Chunks.Count > 0)
        {
            Debug.Log("We have a midi file loaded!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
