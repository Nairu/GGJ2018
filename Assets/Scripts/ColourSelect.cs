using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourSelect : MonoBehaviour
{
    public GameObject ColourPickerRed;
    public GameObject ColourPickerGreen;
    public GameObject ColourPickerBlue;

    private RawImage rawImage;

    private RawImage rawImageRed;
    private RawImage rawImageGreen;
    private RawImage rawImageBlue;

    // Use this for initialization
    void Awake()
    {
        rawImage = GetComponent<RawImage>();

        rawImageRed = ColourPickerRed.GetComponent<RawImage>();
        rawImageGreen = ColourPickerGreen.GetComponent<RawImage>();
        rawImageBlue = ColourPickerBlue.GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        rawImage.color = new Color(rawImageRed.GetComponent<ColourAdjuster>().Red,
            rawImageGreen.GetComponent<ColourAdjuster>().Green, rawImageBlue.GetComponent<ColourAdjuster>().Blue);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ColourPickerRed.GetComponent<ColourAdjuster>().IsSelected = true;
            ColourPickerGreen.GetComponent<ColourAdjuster>().IsSelected = false;
            ColourPickerBlue.GetComponent<ColourAdjuster>().IsSelected = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ColourPickerRed.GetComponent<ColourAdjuster>().IsSelected = false;
            ColourPickerGreen.GetComponent<ColourAdjuster>().IsSelected = true;
            ColourPickerBlue.GetComponent<ColourAdjuster>().IsSelected = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ColourPickerRed.GetComponent<ColourAdjuster>().IsSelected = false;
            ColourPickerGreen.GetComponent<ColourAdjuster>().IsSelected = false;
            ColourPickerBlue.GetComponent<ColourAdjuster>().IsSelected = true;
        }
    }
}
