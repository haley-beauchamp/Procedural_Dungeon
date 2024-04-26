using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static int score = 0;
    private static int balance = 0;

    public static void ItemCollected(int itemPoints)
    {
        UpdateScore(itemPoints);
    }

    public static void ChangeBalance(int balanceIncreaseAmount)
    {
        balance = Mathf.Max(0, balance + balanceIncreaseAmount);
        HUD.instance.UpdateWallet(balance);
    }

    public static int GetBalance()
    {
        return balance;
    }

    public static void EnemyDefeated(int enemyPoints)
    {
        UpdateScore(enemyPoints);
    }

    private static void UpdateScore(int points)
    {
        score += points;
        HUD.instance.UpdateScore(score);
    }

    public static void GameOver()
    {
        Time.timeScale = 0;
        GameOverScreen.instance.DisplayScore(score);
    }

    public static void ResetGame()
    {
        score = 0;
        balance = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}