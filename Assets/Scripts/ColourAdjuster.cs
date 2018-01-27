using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourAdjuster : MonoBehaviour
{
    [System.Serializable]
    public enum TheColour
    {
        Red,
        Green,
        Blue
    }

    public TheColour BlockColour = TheColour.Red;
    [Range(0, 1)]
    public float Speed;

    private Color colour = new Color(1, 0, 0);
    private RawImage rawImage;
    private Vector3 ZoopleColour;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
        
        switch (BlockColour)
        {
            case TheColour.Red:
                ZoopleColour = new Vector3(1, 0, 0);
                break;
            case TheColour.Green:
                ZoopleColour = new Vector3(0, 1, 0);
                break;
            case TheColour.Blue:
                ZoopleColour = new Vector3(0, 0, 1);
                break;
        }
        rawImage.color = new Color(ZoopleColour.x, ZoopleColour.y, ZoopleColour.z);
    }

    void Update()
    {
        Color currColour = rawImage.color;
        Color newColour = currColour;
        float value = 0f;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                value = -Speed;
            else if (Input.GetKey(KeyCode.RightArrow))
                value = Speed;

            Color temp = currColour;

            switch (BlockColour)
            {
                case TheColour.Red:
                    temp.r += value;
                    break;
                case TheColour.Green:
                    temp.g += value;
                    break;
                case TheColour.Blue:
                    temp.b += value;
                    break;
            }

            newColour = Color.Lerp(currColour, temp, Time.deltaTime);

            if (currColour != newColour)
                rawImage.color = newColour;
        }
    }

    Color ConvertRGB(float r, float g, float b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }
}
