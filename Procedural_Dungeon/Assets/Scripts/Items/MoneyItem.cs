using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyItem : Item
{
    [SerializeField] int balanceIncreaseAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            GameManager.ChangeBalance(balanceIncreaseAmount);
            ItemCollected();
        }
    }
}