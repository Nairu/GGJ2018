using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreScreenThing : MonoBehaviour {

    public Sprite S;
    public Sprite A;
    public Sprite B;
    public Sprite C;
    public Sprite D;
    public Sprite E;

    public Image GradeImage;

    public TextMeshProUGUI Perfects;
    public TextMeshProUGUI Goods;
    public TextMeshProUGUI Bads;
    public TextMeshProUGUI Misses;
    public TextMeshProUGUI BestCombo;
    public TextMeshProUGUI Score;

    public Button continueButton;

    ScoreHolder h;

    // Use this for initialization
    void Start () {
        continueButton.Select();

        h = FindObjectOfType<ScoreHolder>();

        Perfects.text = "Perfects: " + h.perfectHits;
        Goods.text = "Goods: " + h.goodHits;
        Bads.text = "Bads: " + h.badHits;
        Misses.text = "Misses: " + h.badHits;
        BestCombo.text = "Best Combo: " + h.maxCombo;
        Score.text = "Final Score: " + h.Score;

        var percentage = h.Percentage;

        if (percentage == 100f)
        {
            GradeImage.sprite = S;
        }
        else if (percentage > 90f)
        {
            GradeImage.sprite = A;
        }
        else if (percentage > 70f)
        {
            GradeImage.sprite = B;
        }
        else if (percentage > 50f)
        {
            GradeImage.sprite = C;
        }
        else if (percentage > 40f)
        {
            GradeImage.sprite = D;
        }
        else
        {
            GradeImage.sprite = E;
        }
	}
	
    public void ReturnToLevelSelect()
    {
        if (h != null)
            Destroy(h.gameObject);

        FindObjectOfType<SoundManager>().Reset();
        SceneManager.LoadScene("LevelSelect");
    }

	// Update is called once per frame
	void Update () {
		
	}
}
