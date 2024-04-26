using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : Item
{
    [SerializeField] int healAmount;
    [SerializeField] bool increasesMaxHealth;
    [SerializeField] int maxHealthIncreaseAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isShopItem && GameManager.GetBalance() < cost) //get your grubby hands off that item
            {
                return;
            } else
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                playerHealth.RestoreHealth(healAmount);
                if (increasesMaxHealth)
                {
                    playerHealth.IncreaseMaxHealth(maxHealthIncreaseAmount);
                }

                ItemCollected();
            }
        }
    }
}
