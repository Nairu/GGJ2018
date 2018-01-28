using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;
using TMPro;

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

    public TextMeshProUGUI TextUI;

    private int score;

    public GameObject NoteA;
    public GameObject NoteB;
    public GameObject NoteX;
    public GameObject NoteY;

    MultiPathManager manager;

    private void Start()
    {
        manager = GetComponent<MultiPathManager>();
    }

    private void OnDrawGizmos()
    {
        noteHitPoint.transform.position = Handles.PositionHandle(noteHitPoint.transform.position, noteHitPoint.transform.rotation);
    }

    float currentTime = 0;

    public List<GameObject> spawnedNotes = new List<GameObject>();

    public void Update()
    {
        if (spawnedNotes.Count == 0)
            return;

        RemoveNullGameobjects();
        CheckForInput();
        CleanupSpawnedNotes();

        TextUI.text = string.Format("Score: {0}", score);
    }

    void RemoveNullGameobjects()
    {
        spawnedNotes.RemoveAll(x => x == null);
    }

    void CheckForInput()
    {
        Transform noteClosest = getClosestNote(spawnedNotes.Select(x => x.transform).Where(x => x.GetComponent<Note>().Perfection == NotePerfection.Unhit).ToArray(), noteHitPoint.transform);

        if (Input.GetButtonDown("A"))
        {
            NotePerfection prf;
            if (noteClosest.GetComponent<Note>().ControllerButton != ButtonEnum.A)
            {
                //THIS ISN'T THE RIGHT NOTE, ABORT!
                prf = NotePerfection.Missed;
            }
            else
            {
                prf = GetNoteHitValue(noteClosest, noteHitPoint.transform);
            }
            noteClosest.GetComponent<Note>().Perfection = prf;
            DoStuffWithNote(noteClosest, prf);
        }
        else if (Input.GetButtonDown("B"))
        {
            NotePerfection prf;
            if (noteClosest.GetComponent<Note>().ControllerButton != ButtonEnum.B)
            {
                //THIS ISN'T THE RIGHT NOTE, ABORT!
                prf = NotePerfection.Missed;
            }
            else
            {
                prf = GetNoteHitValue(noteClosest, noteHitPoint.transform);
            }
            noteClosest.GetComponent<Note>().Perfection = prf;
            DoStuffWithNote(noteClosest, prf);
        }
        else if (Input.GetButtonDown("X"))
        {
            NotePerfection prf;
            if (noteClosest.GetComponent<Note>().ControllerButton != ButtonEnum.X)
            {
                //THIS ISN'T THE RIGHT NOTE, ABORT!
                prf = NotePerfection.Missed;
            }
            else
            {
                prf = GetNoteHitValue(noteClosest, noteHitPoint.transform);
            }
            noteClosest.GetComponent<Note>().Perfection = prf;
            DoStuffWithNote(noteClosest, prf);
        }
        else if (Input.GetButtonDown("Y"))
        {
            NotePerfection prf;
            if (noteClosest.GetComponent<Note>().ControllerButton != ButtonEnum.Y)
            {
                //THIS ISN'T THE RIGHT NOTE, ABORT!
                prf = NotePerfection.Missed;
            }
            else
            {
                prf = GetNoteHitValue(noteClosest, noteHitPoint.transform);
            }
            noteClosest.GetComponent<Note>().Perfection = prf;
            DoStuffWithNote(noteClosest, prf);
        }
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
                DoStuffWithNote(note.transform, note.GetComponent<Note>().Perfection);
                //note.GetComponent<SetC>
            }
        }
    }

    NotePerfection GetNoteHitValue(Transform note, Transform target)
    {
        if (Vector2.Distance(note.position, target.position) < PerfectHitRange)
            return NotePerfection.Perfect;
        else if (Vector2.Distance(note.position, target.position) < OkayHitRange)
            return NotePerfection.Good;
        else if (Vector2.Distance(note.position, target.position) < BadHitRange)
            return NotePerfection.Bad;
        else
        {
            if (note.GetComponent<Note>().direction == PathCreator.Direction.Forward)
                return NotePerfection.Unhit;
            else
                return NotePerfection.Missed;
        }
    }

    private void DoStuffWithNote(Transform note, NotePerfection perfection)
    {
        if (perfection == NotePerfection.Missed)
        {
            note.GetComponent<Note>().SetDeactivated();
        }
        else
        {
            score += perfection == NotePerfection.Perfect ? PerfectScoreMod : (perfection == NotePerfection.Good ? GoodScoreMod : (perfection == NotePerfection.Bad ? BadScoreMod : 0));
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

