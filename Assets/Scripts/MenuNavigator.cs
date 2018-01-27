using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [Space]
    public TextMeshProUGUI StartGameText;
    public TextMeshProUGUI OptionsText;
    public TextMeshProUGUI QuiteGameText;
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (navigationTimeOut >= NavigationDelay)
        {
            float inputY = Input.GetAxisRaw("Vertical");

            if (currMenu == Menu.M_MAIN_MENU)
            {
                if (Input.GetButton("Fire1"))
                {
                    switch (currMenuItem)
                    {
                        case MainMenuItem.MMI_START:
                            // TODO: Load the game scene.
                            Debug.Log("This is when the game would run.");
                            break;
                        case MainMenuItem.MMI_OPTIONS:
                            MainMenuScreen.SetActive(false);
                            OptionsScreen.SetActive(true);
                            currMenu = Menu.M_OPTIONS_MENU;
                            break;
                        case MainMenuItem.MMI_EXIT:
                            Application.Quit();
                            break;
                    }
                }
                else
                {
                    int currentIdx = (int)currMenuItem;
                    
                    if (inputY == 1)
                    {
                        if (currentIdx == 0)
                        {
                            currentIdx = menuItemLength;
                        }
                        else
                        {
                            currentIdx = --currentIdx % menuItemLength;
                        }
                    }
                    else if (inputY == -1)
                    {
                        currentIdx = ++currentIdx % menuItemLength;
                    }
                    
                    if ((MainMenuItem)currentIdx != currMenuItem)
                    {
                        currMenuItem = (MainMenuItem)currentIdx;
                    }
                }
            }
            else if (currMenu == Menu.M_OPTIONS_MENU)
            {
                int currentIdx = (int)currOptionItem;

                if (inputY >= 1)
                    currentIdx = --currentIdx % optionItemLength;
                else if (inputY <= -1)
                    currentIdx = ++currentIdx % optionItemLength;

                if ((OptionsItem)currentIdx != currOptionItem)
                    currOptionItem = (OptionsItem)currentIdx;
            }
            navigationTimeOut = 0f;
        }
        navigationTimeOut += Time.fixedDeltaTime;
    }
}
