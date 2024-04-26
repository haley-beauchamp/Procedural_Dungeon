using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatIncreaseItem : Item
{
    [SerializeField] string buffType;
    [SerializeField] float buffAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isShopItem && GameManager.GetBalance() < cost) //get your grubby hands off that item
            {
                return;
            }
            else
            {
                switch (buffType)
                {
                    case "Damage":
                        collision.gameObject.GetComponent<PlayerAttack>().IncreaseDamageBuff((int) buffAmount);
                        break;
                    case "Attack Speed":
                        collision.gameObject.GetComponent<PlayerAttack>().DecreaseTimeBetweenShots(buffAmount);
                        break;
                    case "Movement Speed":
                        collision.gameObject.GetComponent<PlayerMovement>().IncreaseMovementSpeed(buffAmount);
                        break;
                    default:
                        break;
                }

                ItemCollected();
            }
        }
    }
}
