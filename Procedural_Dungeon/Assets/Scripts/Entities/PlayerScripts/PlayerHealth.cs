using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityHealth
{
    [SerializeField] AudioClip damagedSound;
    [SerializeField] AudioClip deathSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        HUD.instance.UpdateHP(hitPoints);
    }

    public override void DealDamage(int baseDamage)
    {
        base.DealDamage(baseDamage);
        audioSource.PlayOneShot(damagedSound);
        if (hitPoints <= 0)
        {
            audioSource.PlayOneShot(deathSound);
            GameManager.GameOver();
        }
    }

    public void RestoreHealth(int healAmount)
    {
        hitPoints = Mathf.Min(maxHitPoints, hitPoints + healAmount);
    }

    public void IncreaseMaxHealth(int increaseAmount)
    {
        maxHitPoints += increaseAmount;
    }
}