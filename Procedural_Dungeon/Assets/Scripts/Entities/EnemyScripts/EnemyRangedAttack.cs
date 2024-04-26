using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float attackCooldown = 1f;

    private EnemyMovement enemyMovement;
    private GameObject player;
    private bool attackOnCooldown;

    void Start()
    {
        enemyMovement = gameObject.GetComponent<EnemyMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (enemyMovement.isAttacking && !attackOnCooldown)
        {
            Vector2 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.Normalize();
            Vector3 spawnPosition = transform.position + (Vector3) directionToPlayer * 2f;

            Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        attackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        attackOnCooldown = false;
    }
}
