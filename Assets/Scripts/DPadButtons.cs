using UnityEngine;
using System.Collections;

/// <summary> This class maps the DPad axis to buttons. </summary>
public class DPadButtons : MonoBehaviour
{
    public static bool up;
    public static bool down;
    public static bool left;
    public static bool right;

    private float lastX, lastY;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        up = down = left = right = false;
        lastX = lastY = 0;
    }

    void Update()
    {
        float lastDpadX = lastX;
        float lastDpadY = lastY;

        if (Input.GetAxis("DPadX") == 1 || Input.GetAxis("DPadX") == -1)
        {
            float DPadX = Input.GetAxis("DPadX");
            
            if (DPadX == 1 && lastDpadX != 1) { right = true; } else { right = false; }
            if (DPadX == -1 && lastDpadX != -1) { left = true; } else { left = false; }

            lastX = DPadX;
        }
        else { lastX = 0; }

        if (Input.GetAxis("DPadY") == 1 || Input.GetAxis("DPadY") == -1)
        {
            float DPadY = Input.GetAxis("DPadY");
            if (DPadY == 1 && lastDpadY != 1) { up = true; } else { up = false; }
            if (DPadY == -1 && lastDpadY != -1) { down = true; } else { down = false; }

            lastY = DPadY;
        }
        else { lastY = 0; }

        //if we've already triggered a thing, don't remove it because we haven't pressed the button as well
        if (!down) down = Input.GetButtonUp("A");
        if (!up) up = Input.GetButtonUp("Y");
        if (!left) left = Input.GetButtonUp("X");
        if (!right) right = Input.GetButtonUp("B");

        down = Input.GetButtonUp("A");
        up = Input.GetButtonUp("Y");
        left = Input.GetButtonUp("X");
        right = Input.GetButtonUp("B");
    }
}