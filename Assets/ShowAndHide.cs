using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class ShowAndHide : MonoBehaviour {

    public Animation animation;
    public Button ShowButton;

    public Button StartButton;

    private Button buttonThatSummonedUs;
    private SongInfo song;

    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button insaneButton;


    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (StartButton != null)
            StartButton.Select();
    }

    public void ShowPanel(Button sender)
    {
        animation.Play("DifficultAppear");
        ShowButton.Select();
        buttonThatSummonedUs = sender;
        song = buttonThatSummonedUs.GetComponent<SongInfo>();

        easyButton.interactable = song.DifficultiesWeSupport.Contains(SoundManager.SongDifficulty.Easy);
        mediumButton.interactable = song.DifficultiesWeSupport.Contains(SoundManager.SongDifficulty.Medium);
        hardButton.interactable = song.DifficultiesWeSupport.Contains(SoundManager.SongDifficulty.Hard);
        insaneButton.interactable = song.DifficultiesWeSupport.Contains(SoundManager.SongDifficulty.Insane);
    }

    public void HidePanel()
    {
        animation.Play("DifficultyDisappear");
        buttonThatSummonedUs.Select();
    }

    public void PlaySongForDifficulty(int difficulty)
    {
        var manager = FindObjectOfType<SoundManager>();
        manager.difficulty = (SoundManager.SongDifficulty)difficulty;
        manager.MidiName = song.MidiFileName;
        manager.Song = song.SongToPlay;
        SceneManager.LoadScene("Level0");
    }
}
