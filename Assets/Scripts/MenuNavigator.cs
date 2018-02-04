using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject StartButton;
    public GameObject HowToPlayButton;
    public GameObject OptionsButton;
    public GameObject ExitButton;
    [Space]
    public GameObject MainMenuScreen;
    public GameObject OptionsScreen;
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
    
    private void Start()
    {
        
    }
    
    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (currMenu == Menu.M_MAIN_MENU)
    //    {
    //        if (Input.GetButton("Fire1"))
    //        {
    //            if (StartButton.GetComponent<ButtonNotifier>().Selected)
    //            {
    //                Debug.Log("Start pressed");
    //            }
    //            else if (HowToPlayButton.GetComponent<ButtonNotifier>().Selected)
    //            {
    //                Debug.Log("How to play pressed");
    //            }
    //            else if (OptionsButton.GetComponent<ButtonNotifier>().Selected)
    //            {
    //                currMenu = Menu.M_OPTIONS_MENU;
    //                MainMenuScreen.SetActive(false);
    //                OptionsScreen.SetActive(true);
    //            }
    //            else if (ExitButton.GetComponent<ButtonNotifier>().Selected)
    //            {
    //                Debug.Log("Exit pressed");
    //                Application.Quit();
    //            }
    //        }
    //    }
    //    else if (currMenu == Menu.M_OPTIONS_MENU)
    //    {
            
    //    }
    //}

}
