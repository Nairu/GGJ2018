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

    private Path currentPath;
    private PathCreator.Direction direction = PathCreator.Direction.Forward;
    private float posAlongPath;
    private bool finished;

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
                finished = true;
            else
            {
                direction = PathCreator.Direction.Backward;
                posAlongPath = 0;
                currentPath = ExitPath;
            }
        }
    }

}
