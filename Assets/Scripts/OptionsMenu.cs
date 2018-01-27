using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject MainMenu;

    public void BackToMainMenu()
    {
        gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }
}
