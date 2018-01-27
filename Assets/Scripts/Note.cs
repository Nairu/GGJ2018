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

public class Note : MonoBehaviour {

    public ButtonEnum ControllerButton;

    public Path EntryPath;
    public Path ExitPath;

    public string EntryPathName;
    public string ExitPathName;

    private Path currentPath;
    private PathCreator.Direction direction = PathCreator.Direction.Forward;
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
            transform.position = PathCreator.PointOnLine(currentPath, posAlongPath, 1, direction);
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

}
