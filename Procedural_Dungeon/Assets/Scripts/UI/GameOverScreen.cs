using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen instance;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] string defaultString = "Score: ";

    void Start()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void DisplayScore(int score)
    {
        gameObject.SetActive(true);
        scoreText.text = defaultString + score;
    }

    public void OnPlayAgainClicked()
    {
        GameManager.ResetGame();
    }
}