using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ColourAdjuster : MonoBehaviour
{
    public enum TheColour
    {
        Red,
        Green,
        Blue
    }

    public float Red
    {
        get
        {
            return currColour.r;
        }
    }

    public float Green
    {
        get
        {
            return currColour.g;
        }
    }

    public float Blue
    {
        get
        {
            return currColour.b;
        }
    }

    public TheColour BlockColour = TheColour.Red;
    [Range(0, 1)]
    public float Speed;
    public TextMeshProUGUI ProTextMesh;
    public bool IsSelected = false;
    
    private RawImage rawImage;
    private Color currColour;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
        Vector3 colour = Vector3.zero;

        switch (BlockColour)
        {
            case TheColour.Red:
                colour.x = 1;
                break;
            case TheColour.Green:
                colour.y = 1;
                break;
            case TheColour.Blue:
                colour.z = 1;
                break;
        }
        rawImage.color = new Color(colour.x, colour.y, colour.z);
        currColour = rawImage.color;
    }

    void Update()
    {
        if (!IsSelected)
            return;

        Color newColour = currColour;
        float value = 0f;

        float input = Input.GetAxisRaw("Horizontal");

        if (input >= 1 || input <= -1)
        {
            if (input <= -1)
                value = -Speed;
            else if (input >= 1)
                value = Speed;
            value *= Time.deltaTime;

            switch (BlockColour)
            {
                case TheColour.Red:
                    newColour.r = Mathf.Clamp01(currColour.r + value);
                    break;
                case TheColour.Green:
                    newColour.g = Mathf.Clamp01(currColour.g + value);
                    break;
                case TheColour.Blue:
                    newColour.b = Mathf.Clamp01(currColour.b + value);
                    break;
            }

            if (currColour != newColour)
            {
                currColour = newColour;
                int rgbValue = 0;

                switch (BlockColour)
                {
                    case TheColour.Red:
                        rgbValue = ConvertToTwoFiveFive(newColour.r);
                        break;
                    case TheColour.Green:
                        rgbValue = ConvertToTwoFiveFive(newColour.g);
                        break;
                    case TheColour.Blue:
                        rgbValue = ConvertToTwoFiveFive(newColour.b);
                        break;
                }
                ProTextMesh.text = rgbValue.ToString();
            }
        }
    }

    public int ConvertToTwoFiveFive(float value)
    {
        return Convert.ToInt32(255 * value);
    }
}
