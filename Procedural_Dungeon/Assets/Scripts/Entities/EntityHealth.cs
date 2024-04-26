using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] protected int maxHitPoints;
    [SerializeField] int hitPointRestoreInterval;
    [SerializeField] int hitPointRestoreAmount;

    protected int hitPoints;

    void Awake()
    {
        hitPoints = maxHitPoints;
        StartCoroutine(RestoreHealth());
    }

    public virtual void DealDamage(int baseDamage)
    {
        hitPoints -= baseDamage;
    }

    private IEnumerator RestoreHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(hitPointRestoreInterval);

            hitPoints = Mathf.Min(maxHitPoints, hitPoints + hitPointRestoreAmount);
        }
    }
}