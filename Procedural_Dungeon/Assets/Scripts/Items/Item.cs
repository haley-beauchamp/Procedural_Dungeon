using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected int itemScore;
    [SerializeField] protected int cost;
    public bool isShopItem;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected void ItemCollected()
    {
        GameManager.ItemCollected(itemScore);
        GameManager.ChangeBalance(-cost);
        AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        Destroy(gameObject);
    }
}