using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] int baseAttackDamage;
    private bool attackOnCooldown;
    private readonly float attackCooldown = 2f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !attackOnCooldown)
        {
            StartCoroutine(AttackCooldown());
            collision.gameObject.GetComponent<PlayerHealth>().DealDamage(baseAttackDamage);
        }
    }
    
    private IEnumerator AttackCooldown()
    {
        attackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        attackOnCooldown = false;
    }
}