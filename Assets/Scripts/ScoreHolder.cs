using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHolder : MonoBehaviour {

    public int maxCombo;
    public int perfectHits;
    public int goodHits;
    public int badHits;
    public int missedHits;
    public int TotalNotes;
    public int Score;

    public float Percentage
    {
        get
        {
            return (((float)perfectHits + (float)goodHits + (float)badHits) / (float)TotalNotes) * 100f;
        }
    }

}
