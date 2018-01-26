using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {
    public bool A;
    public bool B;
    public bool X;
    public bool Y;

    public bool Held;
    [Range(0, 10f)]
    public float HeldTime;

    [SerializeField]
    private GameObject A_GO;
    [SerializeField]
    private GameObject B_GO;
    [SerializeField]
    private GameObject X_GO;
    [SerializeField]
    private GameObject Y_GO;

    private void Update()
    {
        A_GO.SetActive(A);
        B_GO.SetActive(B);
        X_GO.SetActive(X);
        Y_GO.SetActive(Y);
    }
}
