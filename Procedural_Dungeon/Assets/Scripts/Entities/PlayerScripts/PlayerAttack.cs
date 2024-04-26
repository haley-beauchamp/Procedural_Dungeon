using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float timeBetweenShots = 1;
    [SerializeField] AudioClip attackSound;

    private AudioSource audioSource;
    private Vector2 shootPostion;
    private bool canShoot = true;
    private string lastDirectionKeyPressed = "Down";

    private int damageBuff;
    private float minimumAttackCooldown = 0.5f;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            lastDirectionKeyPressed = "Left";
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            lastDirectionKeyPressed = "Right";
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            lastDirectionKeyPressed = "Up";
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            lastDirectionKeyPressed = "Down";
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canShoot)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        if (lastDirectionKeyPressed == "Left")
        {
            shootPostion = transform.position + Vector3.left * 2;
        }
        else if (lastDirectionKeyPressed == "Right")
        {
            shootPostion = transform.position + Vector3.right * 2;
        }
        else if (lastDirectionKeyPressed == "Up")
        {
            shootPostion = transform.position + Vector3.up * 2;
        }
        else if (lastDirectionKeyPressed == "Down")
        {
            shootPostion = transform.position + Vector3.down * 2;
        }

        audioSource.PlayOneShot(attackSound);
        GameObject bullet = Instantiate(bulletPrefab, shootPostion, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetBulletPath(lastDirectionKeyPressed);
        bulletScript.IncreaseBulletDamage(damageBuff);
        StartCoroutine(ShootCooldown());
    }

    IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    public void DecreaseTimeBetweenShots(float timeToTakeOff)
    {
        timeBetweenShots = Mathf.Max(minimumAttackCooldown, timeBetweenShots - timeToTakeOff);
    }

    public void IncreaseDamageBuff(int damageIncreaseAmount)
    {
        damageBuff += damageIncreaseAmount;
    }
}