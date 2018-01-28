using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    public enum Menu
    {
        M_MAIN_MENU,
        M_OPTIONS_MENU
    }

    public enum MainMenuItem
    {
        MMI_START,
        MMI_OPTIONS,
        MMI_EXIT
    }

    public enum OptionsItem
    {
        OI_BUTTON_A,
        OI_BUTTON_B,
        OI_BUTTON_X,
        OI_BUTTON_Y
    }
    public GameObject MainMenuScreen;
    public GameObject OptionsScreen;
    public GameObject GameLogo;
    [Space]
    public GameObject StartButton;
    public GameObject HowToPlayButton;
    public GameObject OptionsButton;
    public GameObject ExitButton;
    [Space]
    public GameObject ColourBlindButton;
    public GameObject VolumeButton;
    public GameObject BackButton;
    [Space]
    public GameObject ColourPicker;
    public GameObject ButtonA;
    [Space]
    public float NavigationDelay = 0.5f;

    private float navigationTimeOut = 0f;

    private Menu currMenu = Menu.M_MAIN_MENU;
    private MainMenuItem currMenuItem = MainMenuItem.MMI_START;
    private OptionsItem currOptionItem = OptionsItem.OI_BUTTON_A;

    private int menuItemLength;
    private int optionItemLength;

    private void Awake()
    {
        menuItemLength = Enum.GetNames(typeof(MainMenuItem)).Length;
        optionItemLength = Enum.GetNames(typeof(OptionsItem)).Length;
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            if (currMenu == Menu.M_MAIN_MENU)
            {
                if (StartButton.GetComponent<ButtonNotifier>().Selected)
                {
                    Debug.Log("Start pressed");
                }
                else if (HowToPlayButton.GetComponent<ButtonNotifier>().Selected)
                {
                    Debug.Log("How to play pressed");
                }
                else if (OptionsButton.GetComponent<ButtonNotifier>().Selected)
                {
                    currMenu = Menu.M_OPTIONS_MENU;
                    MainMenuScreen.SetActive(false);
                    GameLogo.SetActive(false);
                    OptionsScreen.SetActive(true);
                    OptionsButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                    ColourBlindButton.GetComponent<Button>().Select();
                }
                else if (ExitButton.GetComponent<ButtonNotifier>().Selected)
                {
                    Debug.Log("Exit pressed");
                    Application.Quit();
                }
            }
            else if (currMenu == Menu.M_OPTIONS_MENU)
            {
                if (ColourBlindButton.GetComponent<ButtonNotifier>().Selected)
                {
                    Debug.Log("Colour blind pressed");
                    ButtonA.GetComponent<Button>().Select();

                    ColourBlindButton.GetComponent<Button>().interactable = false;
                    VolumeButton.GetComponent<Button>().interactable = false;
                    BackButton.GetComponent<Button>().interactable = false;
                }
                else if (VolumeButton.GetComponent<ButtonNotifier>().Selected)
                {
                    Debug.Log("Volume pressed");
                }
                else if (BackButton.GetComponent<ButtonNotifier>().Selected)
                {
                    currMenu = Menu.M_MAIN_MENU;
                    MainMenuScreen.SetActive(true);
                    GameLogo.SetActive(true);
                    OptionsScreen.SetActive(false);
                    BackButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                    StartButton.GetComponent<Button>().Select();
                }
                else if (ButtonA.GetComponent<ButtonNotifier>().Selected)
                {
                    ButtonA.GetComponent<Button>().interactable = false;

                    ColourPicker.GetComponentsInChildren<Button>()[0].Select();
                }
            }
        }
    }
}
