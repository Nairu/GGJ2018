using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InputHelper
{
    bool A;
    bool B;
    bool C;
    bool D;
}

public class NoteSpawner : MonoBehaviour
{
    public GameObject noteHitPoint;
    public Vector3 noteHitPosition
    {
        get { return m_noteHitPosition; }
        set { m_noteHitPosition = value; }
    }
    private Vector3 m_noteHitPosition;

    //public TextM mesh;

    [Range(0, 0.1f)]
    public float PerfectHitRange;
    [Range(0, 0.25f)]
    public float OkayHitRange;
    [Range(0, 0.5f)]
    public float BadHitRange;

    public int PerfectScoreMod = 5;
    public int GoodScoreMod = 3;
    public int BadScoreMod = 1;

    public int NumberOfNotes = 0;

    public TextMeshProUGUI ScoreUI;
    public TextMeshProUGUI ComboUI;

    public GameObject AxomStem;

    private int score = 0;
    private int combo = 0;

    public GameObject NoteA;
    public GameObject NoteB;
    public GameObject NoteX;
    public GameObject NoteY;

    MultiPathManager manager;

    private bool songFinished;

    private int maxCombo;
    private int perfectHits;
    private int goodHits;
    private int badHits;
    private int missedHits;
    
    private void Start()
    {
        manager = GetComponent<MultiPathManager>();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        noteHitPoint.transform.position = Handles.PositionHandle(noteHitPoint.transform.position, noteHitPoint.transform.rotation);
    }
#endif

    float currentTime = 0;

    public List<GameObject> spawnedNotes = new List<GameObject>();

    public void Update()
    {
        if (spawnedNotes.Count == 0)
            return;

        RemoveNullGameobjects();

        List<Note> notes = PopulateClosestNotes();
        if (notes.Count > 0 && ValidateHitNotes(notes))
        {
            //get the first note because now we know that they are all valid, so they should all be hit at the same time.
            Note note = notes[0];

            //lets calculate some points!
            SetNoteHitValues(ref notes);

            if (notes[0].Perfection == NotePerfection.Unhit)
                return;

            //now lets do something with them!
            score += (int)CalculatePointsForNotes(notes);

            if (notes[0].Perfection == NotePerfection.Perfect) perfectHits += notes.Count;
            else if (notes[0].Perfection == NotePerfection.Good) goodHits += notes.Count;
            else if (notes[0].Perfection == NotePerfection.Bad) badHits += notes.Count;
            combo += notes.Count;

            if (note.Perfection != NotePerfection.Unhit)
            {
                foreach (Note n in notes)
                    n.NoteHit();
                if (note.Perfection == NotePerfection.Perfect)
                {
                    AxomStem.GetComponent<Animation>().Play("StemGoodAnimation");
                }
            }
        }
        else if (notes.Count > 0)
        {
            for (int i = 0; i < notes.Count; i++)
            {
                notes[i].SetDeactivated();
            }
            AxomStem.GetComponent<Animation>().Play("StemBadAnimation");
            combo = 0;
            missedHits += notes.Count;
        }

        CleanupSpawnedNotes();

        ScoreUI.text = string.Format("SCORE: {0}", score);
        ComboUI.text = string.Format("COMBO: {0}", combo);

        if (maxCombo < combo)
            maxCombo = combo;
    }

    public void SongFinished()
    {
        //
        GameObject scoreHold = new GameObject();
        ScoreHolder hd = scoreHold.AddComponent<ScoreHolder>();

        hd.maxCombo = maxCombo;
        hd.TotalNotes = NumberOfNotes;
        hd.perfectHits = perfectHits;
        hd.goodHits = goodHits;
        hd.badHits = badHits;
        hd.missedHits = missedHits;
        hd.Score = score;

        //we need to load the score screen here, but for now just go back to the level select.
        DontDestroyOnLoad(scoreHold);

        SceneManager.LoadScene("ScoreScreen");
    }

    void RemoveNullGameobjects()
    {
        spawnedNotes.RemoveAll(x => x == null);
    }
    
    List<Note> PopulateClosestNotes()
    {
        //TODO: Fix this so that it only gets the notes that are "approaching" based on their CurrentPosition when above 0 (and less than 1)
        List<Note> closestNotes = new List<Note>();
        for (int i = 0; i < spawnedNotes.Count; i++)
        {
            Note currentNote = spawnedNotes[i].GetComponent<Note>();

            if (currentNote.Activated == false || currentNote.Perfection != NotePerfection.Unhit)
                continue;
            else if (closestNotes.Count == 0)
                closestNotes.Add(currentNote);
            else if (closestNotes[0] == currentNote)
                continue;
            else
            {
                if (currentNote.CurrentPosition == closestNotes[0].CurrentPosition)
                {
                    closestNotes.Add(currentNote);
                }
                else if (currentNote.CurrentPosition < closestNotes[0].CurrentPosition)
                {
                    //this is closer, wipe the list and start again
                    closestNotes.Clear();
                    closestNotes.Add(currentNote);
                }
            }
        }

        return closestNotes;
    }

    float CalculatePointsForNote(Note note)
    {
        if (note.Perfection == NotePerfection.Perfect) perfectHits++;
        else if (note.Perfection == NotePerfection.Good) goodHits++;
        else if (note.Perfection == NotePerfection.Bad) badHits++;

        var addBase = note.Perfection == NotePerfection.Perfect ? PerfectScoreMod : (note.Perfection == NotePerfection.Good ? GoodScoreMod : (note.Perfection == NotePerfection.Bad ? BadScoreMod : 0));
        addBase *= (combo == 0) ? 1 : (combo % 10 + 1);
        combo++;

        return addBase;
    }

    float CalculatePointsForNotes(List<Note> notes)
    {
        float cumulativeScore = 0;
        foreach (Note note in notes)
            cumulativeScore += CalculatePointsForNote(note);

        return cumulativeScore;
    }

    void CheckForInput()
    {
        //if (Input.GetButtonDown("A"))
        //{
        //    NotePerfection prf;
        //    if (noteClosest.GetComponent<Note>().ControllerButton != ButtonEnum.A)
        //    {
        //        //THIS ISN'T THE RIGHT NOTE, ABORT!
        //        prf = NotePerfection.Missed;
        //    }
        //    else
        //    {
        //        prf = GetNoteHitValue(noteClosest, noteHitPoint.transform);
        //    }
        //    noteClosest.GetComponent<Note>().Perfection = prf;
        //    DoStuffWithNote(noteClosest, prf);
        //}
        //else if (Input.GetButtonDown("B"))
        //{
        //    NotePerfection prf;
        //    if (noteClosest.GetComponent<Note>().ControllerButton != ButtonEnum.B)
        //    {
        //        //THIS ISN'T THE RIGHT NOTE, ABORT!
        //        prf = NotePerfection.Missed;
        //    }
        //    else
        //    {
        //        prf = GetNoteHitValue(noteClosest, noteHitPoint.transform);
        //    }
        //    noteClosest.GetComponent<Note>().Perfection = prf;
        //    DoStuffWithNote(noteClosest, prf);
        //}
        //else if (Input.GetButtonDown("X"))
        //{
        //    NotePerfection prf;
        //    if (noteClosest.GetComponent<Note>().ControllerButton != ButtonEnum.X)
        //    {
        //        //THIS ISN'T THE RIGHT NOTE, ABORT!
        //        prf = NotePerfection.Missed;
        //    }
        //    else
        //    {
        //        prf = GetNoteHitValue(noteClosest, noteHitPoint.transform);
        //    }
        //    noteClosest.GetComponent<Note>().Perfection = prf;
        //    DoStuffWithNote(noteClosest, prf);
        //}
        //else if (Input.GetButtonDown("Y"))
        //{
        //    NotePerfection prf;
        //    if (noteClosest.GetComponent<Note>().ControllerButton != ButtonEnum.Y)
        //    {
        //        //THIS ISN'T THE RIGHT NOTE, ABORT!
        //        prf = NotePerfection.Missed;
        //    }
        //    else
        //    {
        //        prf = GetNoteHitValue(noteClosest, noteHitPoint.transform);
        //    }
        //    noteClosest.GetComponent<Note>().Perfection = prf;
        //    DoStuffWithNote(noteClosest, prf);
        //}
    }

    //TODO: Remove.
    bool ValidateHitNote(Note note)
    {
        if (note.ControllerButton == ButtonEnum.A && ((DPadButtons.left || DPadButtons.right || DPadButtons.up) && note.CurrentPosition < BadHitRange)) {
            return false;
        }
        else if (note.ControllerButton == ButtonEnum.B && ((DPadButtons.left || DPadButtons.down || DPadButtons.up) && note.CurrentPosition < BadHitRange)) {
            return false;
        }
        else if (note.ControllerButton == ButtonEnum.X && ((DPadButtons.down || DPadButtons.right || DPadButtons.up) && note.CurrentPosition < BadHitRange)) {
            return false;
        }
        else if (note.ControllerButton == ButtonEnum.Y && ((DPadButtons.left || DPadButtons.right || DPadButtons.down) && note.CurrentPosition < BadHitRange)) {
            return false;
        }
        else return true;
    }
    
    //TODO: Modify this to get a list of all "required" hit directions, and then pass check that those directions are hit exclusively (fail if up is held when need down e.g)
    bool ValidateHitNotes(List<Note> notes)
    {
        foreach (var note in notes)
        {
            bool noteVal = ValidateHitNote(note);
            if (!noteVal) return false;
        }
        return true;
    }

    void CleanupSpawnedNotes()
    {
        foreach (var note in spawnedNotes)
        {
            var noteNote = note.GetComponent<Note>();

            if (noteNote.direction == PathCreator.Direction.Forward || noteNote.Perfection != NotePerfection.Unhit)
                continue;

            if (Vector2.Distance(note.transform.position, noteHitPoint.transform.position) > BadHitRange)
            {
                note.GetComponent<Note>().Perfection = NotePerfection.Missed;
                //DoStuffWithNote(note.transform, note.GetComponent<Note>().Perfection);
                //note.GetComponent<SetC>
            }
        }
    }

    //TODO: Fix this (its broken and I don't know why)
    void SetNoteHitValues(ref List<Note> notes)
    {
        for (int i = 0; i < notes.Count; i++)
        {
            notes[i].Perfection = SetNoteHitValue(notes[i]);
        }
    }

    //TODO: Fix this (its broken and I don't know why)
    NotePerfection SetNoteHitValue(Note note)
    {
        if (note.CurrentPosition < PerfectHitRange)
            return NotePerfection.Perfect;
        else if (note.CurrentPosition < OkayHitRange)
            return NotePerfection.Good;
        else if (note.CurrentPosition < BadHitRange)
            return NotePerfection.Bad;
        else
            return (note.direction == PathCreator.Direction.Forward ? NotePerfection.Unhit : NotePerfection.Missed);
    }

    private void DoStuffWithNote(Note note, NotePerfection perfection)
    {
        if (perfection == NotePerfection.Missed)
        {
            AxomStem.GetComponent<Animation>().Play("StemBadAnimation");

            note.GetComponent<Note>().SetDeactivated();
            combo = 0;
            missedHits++;
        }
        else
        {
            if (perfection == NotePerfection.Perfect) perfectHits++;
            else if (perfection == NotePerfection.Good) goodHits++;
            else if (perfection == NotePerfection.Bad) badHits++;

            var addBase = perfection == NotePerfection.Perfect ? PerfectScoreMod : (perfection == NotePerfection.Good ? GoodScoreMod : (perfection == NotePerfection.Bad ? BadScoreMod : 0));
            addBase *= (combo == 0) ? 1 : (combo % 10 + 1);
            score += addBase;

            combo++;

            if (perfection != NotePerfection.Unhit)
            {
                note.GetComponent<Note>().NoteHit();

                if (perfection == NotePerfection.Perfect)
                {
                    AxomStem.GetComponent<Animation>().Play("StemGoodAnimation");
                }
            }        
        }
    }

    Transform getClosestNote(Transform[] notes, Transform target)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = target.position;
        foreach (Transform potentialTarget in notes)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    //Spawn note is what gets called from the audio manager to convert one of the 
    //midi notes into a point on the screen. Consider modifying to take in a "chord"
    public void SpawnNote(ButtonEnum num)
    {
        var startPath = manager.GetPathAtIndex(Random.Range(0, manager.PathCount));
        var endPath = manager.GetPathAtIndex(Random.Range(0, manager.PathCount));
        if (endPath == startPath)
        {
            while (endPath == startPath)
                endPath = manager.GetPathAtIndex(Random.Range(0, manager.PathCount));
        }
        GameObject note;
        Note n;
        switch (num)
        {
            case ButtonEnum.A:
                note = NoteA;
                n = note.GetComponent<Note>();
                break;
            case ButtonEnum.B:
                note = NoteB;
                n = note.GetComponent<Note>();
                break;
            case ButtonEnum.X:
                note = NoteX;
                n = note.GetComponent<Note>();
                break;
            case ButtonEnum.Y:
                note = NoteY;
                n = note.GetComponent<Note>();
                break;
            default:
                Debug.LogError("SOMETHING IS VERY VERY WRONG!");
                n = new Note();
                note = new GameObject();
                break;
        }
        n.EntryPath = startPath.path;
        n.ExitPath = endPath.path;
        n.EntryPathName = startPath.gameObject.name;
        n.ExitPathName = endPath.gameObject.name;
        GameObject instance = Instantiate(note, new Vector3(100, 100), Quaternion.identity, transform);
        spawnedNotes.Add(instance);
    }
}

