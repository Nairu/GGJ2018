using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonEnum
{
    A,
    B,
    X,
    Y
}

public enum NotePerfection
{
    Unhit,
    Perfect,
    Good,
    Bad,
    Missed
}

public class Note : MonoBehaviour {

    public ButtonEnum ControllerButton;
    public NotePerfection Perfection = NotePerfection.Unhit;
    public PathCreator.Direction direction = PathCreator.Direction.Forward;

    public Path EntryPath;
    public Path ExitPath;

    public string EntryPathName;
    public string ExitPathName;

    public SpriteRenderer[] renderers;

    private Path currentPath;
    private float posAlongPath;
    private bool finished;

    private void Start()
    {
        currentPath = EntryPath;
    }

    private void Update()
    {
        if (finished)
            return;

        if (posAlongPath <= 1)
        {
            posAlongPath += Time.deltaTime;
            transform.position = PathCreator.PointOnLine(currentPath, posAlongPath, 1.5f, direction);
        }
        else
        {
            if (currentPath == ExitPath)
            {
                transform.position = new Vector2(100, 100);
                Destroy(gameObject);
            }
            else
            {
                direction = PathCreator.Direction.Backward;
                posAlongPath = 0;
                currentPath = ExitPath;
            }
        }
    }

    public void SetDeactivated()
    {
        foreach (var renderer in renderers)
        {
            renderer.color = Color.grey;
        }
    }
}
