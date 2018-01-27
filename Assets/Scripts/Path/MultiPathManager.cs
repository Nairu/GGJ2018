using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//
[System.Serializable]
public class MultiPathManager : MonoBehaviour {

    //handles creating multiple paths and storing them all in one object.
    [HideInInspector, SerializeField] private PathCreator[] paths;

    public int PathCount { get { return paths.Length; } }

    public int GetPathIndex(PathCreator pc)
    {
        return paths.ToList().FindIndex(x => x == pc);
    }

    public PathCreator GetPathAtIndex(int i)
    {
        if (i < 0 || i >= paths.Length)
        {
            Debug.LogError("Passed index out of bounds!");
            return null;
        }
        return paths[i];
    }

    public void ResetPaths()
    {
        paths = new PathCreator[0];
    }
}
