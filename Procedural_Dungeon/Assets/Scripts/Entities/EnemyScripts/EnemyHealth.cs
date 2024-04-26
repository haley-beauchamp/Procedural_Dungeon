using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : EntityHealth
{
    [SerializeField] bool isBoss;
    [SerializeField] int score;
    [SerializeField] GameObject itemDrop;

    public override void DealDamage(int baseDamage)
    {
        base.DealDamage(baseDamage);
        if (hitPoints <= 0)
        {
            GameManager.EnemyDefeated(score);
            if (isBoss)
            {
                GameManager.GameOver();
            }

            if (itemDrop != null)
            {
                Instantiate(itemDrop, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);
        }
    }
}