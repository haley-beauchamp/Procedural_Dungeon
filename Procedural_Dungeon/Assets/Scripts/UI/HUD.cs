using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    [SerializeField] TextMeshProUGUI scoreDisplay;
    [SerializeField] string defaultScoreString = "Score: ";
    [SerializeField] TextMeshProUGUI hpDisplay;
    [SerializeField] string defaultHPString = "Score: ";
    [SerializeField] TextMeshProUGUI walletDisplay;
    [SerializeField] string defaultWalletString = "Coins: ";

    void Start()
    {
        instance = this;
    }

    public void UpdateScore(int score)
    {
        scoreDisplay.text = defaultScoreString + score;
    }

    public void UpdateHP(int hp)
    {
        hpDisplay.text = defaultHPString + hp;
    }

    public void UpdateWallet(int amount)
    {
        walletDisplay.text = defaultWalletString + amount;
    }
}