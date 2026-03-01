using UnityEngine;
using System.Collections;

public class Player_script : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Life System")]
    public int maxLives = 3;
    private int currentLives;

    [Header("Invincibility")]
    public float invincibilityDuration = 1.5f;
    private bool isInvincible = false;

    [Header("Game State")]
    public bool GameOver = false;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
        currentLives = maxLives;
    }

    void Update()
    {
        if (!GameOver)
        {
            MovePlayer();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void MovePlayer()
    {
        float yVelocity = rb.linearVelocity.y;

        if (isGrounded)
        {
            yVelocity = 0f;
        }

        rb.linearVelocity = new Vector2(moveSpeed, yVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!isInvincible)
            {
                TakeDamage();
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void TakeDamage()
    {
        currentLives--;

        if (currentLives <= 0)
        {
            GameOver = true;
            return;
        }

        StartCoroutine(InvincibilityCoroutine());
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(invincibilityDuration);
        spriteRenderer.color = originalColor;

        isInvincible = false;
    }
}
