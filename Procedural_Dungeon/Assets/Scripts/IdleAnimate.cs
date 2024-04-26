using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimate : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private readonly float timeBetweenSprites = 0.25f;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(AnimateSprite());
    }

    private IEnumerator AnimateSprite()
    {
        int currentSprite = 0;

        while (true)
        {
            spriteRenderer.sprite = sprites[currentSprite];
            currentSprite = (currentSprite + 1) % sprites.Length; 
            yield return new WaitForSeconds(timeBetweenSprites);
        }
    }
}