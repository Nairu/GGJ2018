using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject OptionsScreen;

    public void StartGame()
    {
        // TODO: Load the game scene.
        Debug.Log("This is when the game would run.");
    }

    public void Options()
    {
        gameObject.SetActive(false);
        OptionsScreen.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
