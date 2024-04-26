using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5;
    [SerializeField] Sprite[] walkDownSprites;
    [SerializeField] Sprite[] walkHorizontalSprites;
    [SerializeField] Sprite[] walkUpSprites;
    [SerializeField] float timeBetweenAnimationSprites = 0.5f;

    private Rigidbody2D player;
    private bool isFacingRight;
    private SpriteRenderer spriteRenderer;
    private float spriteTimer;
    private int currentSprite;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        Sprite[] currentSprites = walkDownSprites;
        player.velocity = new Vector2(horizontal * movementSpeed, vertical * movementSpeed);

        if (horizontal != 0 || vertical != 0)
        {
            if (horizontal != 0)
            {
                currentSprites = walkHorizontalSprites;
                if (horizontal > 0)
                {
                    isFacingRight = true;
                }
                else
                {
                    isFacingRight = false;
                }
            }
            else if (vertical > 0)
            {
                currentSprites = walkUpSprites;
            }
            else if (vertical < 0)
            {
                currentSprites = walkDownSprites;
            }

            spriteRenderer.flipX = isFacingRight;
            UpdateSprite(currentSprites);
        }
    }

    void UpdateSprite(Sprite[] currentSprites)
    {
        spriteTimer += Time.deltaTime;

        if (spriteTimer >= timeBetweenAnimationSprites)
        {
            spriteTimer = 0f;
            currentSprite = (currentSprite + 1) % currentSprites.Length;
            spriteRenderer.sprite = currentSprites[currentSprite];
        }
    }

    public void IncreaseMovementSpeed(float movementSpeedIncrease)
    {
        movementSpeed += movementSpeedIncrease;
    }
}