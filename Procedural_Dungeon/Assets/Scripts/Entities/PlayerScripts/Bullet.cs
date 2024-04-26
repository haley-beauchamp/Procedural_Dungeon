using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] int bulletDamage;

    private Rigidbody2D bullet;
    private string bulletDirection;

    void Start()
    {
        bullet = GetComponent<Rigidbody2D>();
    }

    public void SetBulletPath(string bulletDirection)
    {
        this.bulletDirection = bulletDirection;
    }

    public void IncreaseBulletDamage(int damageIncreaseAmount)
    {
        bulletDamage += damageIncreaseAmount;
    }

    void FixedUpdate()
    {
        if (bulletDirection == "Left")
        {
            bullet.velocity = new Vector2(-bulletSpeed, 0);
        }
        else if (bulletDirection == "Right")
        {
            bullet.velocity = new Vector2(bulletSpeed, 0);
        }
        else if (bulletDirection == "Up")
        {
            bullet.velocity = new Vector2(0, bulletSpeed);
        }
        else if (bulletDirection == "Down")
        {
            bullet.velocity = new Vector2(0, -bulletSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().DealDamage(bulletDamage);
        }

        Destroy(gameObject);
    }
}