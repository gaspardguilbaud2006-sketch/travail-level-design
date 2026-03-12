using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class dragOnZQSD : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 8f;
    public float acceleration = 5f;
    public float deceleration = 5f;

    public GameObject player;

    private Rigidbody2D rb;
    private Collider2D platformCollider;
    public Collider2D playerCollider;

    bool playerTouchingPlatform;
    bool playerTouchingCeiling;
    bool playerTouchingGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        platformCollider = GetComponent<Collider2D>();
        playerCollider = player.GetComponent<Collider2D>();

        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void FixedUpdate()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) input.y += 1f;
        if (Keyboard.current.sKey.isPressed) input.y -= 1f;
        if (Keyboard.current.aKey.isPressed) input.x -= 1f;
        if (Keyboard.current.dKey.isPressed) input.x += 1f;

        // Détection collisions du player
        playerTouchingPlatform = playerCollider.IsTouching(platformCollider);

        playerTouchingCeiling = false;
        playerTouchingGround = false;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            playerCollider.bounds.center,
            playerCollider.bounds.size,
            0f
        );

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Plafond"))
                playerTouchingCeiling = true;

            if (hit.CompareTag("Ground"))
                playerTouchingGround = true;
        }

        // Bloquer mouvement vertical si le player est coincé
        if (playerTouchingPlatform)
        {
            if (playerTouchingCeiling && input.y > 0)
                input.y = 0;

            if (playerTouchingGround && input.y < 0)
                input.y = 0;
        }

        if (input != Vector2.zero)
        {
            rb.AddForce(input.normalized * acceleration);
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }
        else
        {
            rb.linearVelocity = Vector2.MoveTowards(
                rb.linearVelocity,
                Vector2.zero,
                deceleration * Time.fixedDeltaTime
            );
        }
    }
}