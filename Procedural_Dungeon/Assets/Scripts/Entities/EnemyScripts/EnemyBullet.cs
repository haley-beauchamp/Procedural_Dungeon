using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] int bulletDamage;
    [SerializeField] float bulletLifespanDuration;

    private GameObject player;
    private float lifespanTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        Vector3 direction = (player.transform.position + (Vector3.up * 0.5f) - transform.position).normalized;
        transform.Translate(bulletSpeed * Time.fixedDeltaTime * direction);

        lifespanTimer += Time.fixedDeltaTime;
        if (lifespanTimer >= bulletLifespanDuration)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().DealDamage(bulletDamage);
        } else if (collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        Destroy(gameObject);
    }
}