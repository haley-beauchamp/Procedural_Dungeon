using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float enemySpeed;
    [SerializeField] float detectionRadius = 8;
    [SerializeField] float stoppingDistance;

    private Rigidbody2D enemy;
    private GameObject player;

    private bool travellingLeft = false;
    private bool isFollowingPlayer = false;
    public bool isAttacking = false;

    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= stoppingDistance)
        {
            isFollowingPlayer = false;
            isAttacking = true;
            enemy.velocity = Vector2.zero;
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            isFollowingPlayer = true;
            isAttacking = false;
        }
        else
        {
            isFollowingPlayer = false;
            isAttacking = false;
        }

        if (isFollowingPlayer)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            enemy.velocity = direction * enemySpeed;
            Flip();
        }
    }

    public void Flip()
    {
        // if already facing player while following
        if (isFollowingPlayer && ((player.transform.position.x > enemy.position.x && !travellingLeft) ||
            (player.transform.position.x < enemy.position.x && travellingLeft)))
        {
            return;
        }
        else
        {
            travellingLeft = !travellingLeft;
            gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, detectionRadius);
    //}
}