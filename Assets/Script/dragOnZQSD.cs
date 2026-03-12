using UnityEngine;
using UnityEngine.InputSystem;

public enum ObjectType { Wall, Platform }

[RequireComponent(typeof(Rigidbody2D))]
public class KeyboardMovable : MonoBehaviour
{
    [Header("Type")]
    public ObjectType objectType = ObjectType.Platform;

    [Header("Movement")]
    public float maxSpeed = 8f;
    public float acceleration = 5f;
    public float deceleration = 5f;

    [Header("Player Reference")]
    public GameObject player;

    private Rigidbody2D rb;
    private Collider2D objectCollider;
    private Collider2D playerCollider;

    private bool playerTouchingThis;
    private bool playerTouchingCeiling;
    private bool playerTouchingGround;

    private Vector2 currentVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        objectCollider = GetComponent<Collider2D>();

        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        if (player != null)
            playerCollider = player.GetComponent<Collider2D>();
        else
            Debug.LogWarning("Player non assigné sur " + gameObject.name);
    }

    void FixedUpdate()
    {
        float input = ReadInput();

        playerTouchingThis = playerCollider != null && playerCollider.IsTouching(objectCollider);
        playerTouchingCeiling = false;
        playerTouchingGround = false;

        if (playerTouchingThis && objectType == ObjectType.Wall)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(
                playerCollider.bounds.center,
                playerCollider.bounds.size,
                0f
            );

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Plafond")) playerTouchingCeiling = true;
                if (hit.CompareTag("Ground")) playerTouchingGround = true;
            }

            if (playerTouchingCeiling && input > 0f) input = 0f;
            if (playerTouchingGround && input < 0f) input = 0f;
        }

        Vector2 direction = objectType == ObjectType.Wall
            ? new Vector2(0f, input)
            : new Vector2(input, 0f);

        // RESET VELOCITY SI ON CHANGE DE DIRECTION
        if (direction.x != 0 && Mathf.Sign(direction.x) != Mathf.Sign(currentVelocity.x))
            currentVelocity.x = 0;

        if (direction.y != 0 && Mathf.Sign(direction.y) != Mathf.Sign(currentVelocity.y))
            currentVelocity.y = 0;

        // Accélération
        if (direction != Vector2.zero)
        {
            currentVelocity += direction * acceleration * Time.fixedDeltaTime;
            currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed);
        }
        else
        {
            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                Vector2.zero,
                deceleration * Time.fixedDeltaTime
            );
        }

        Vector2 move = currentVelocity * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    float ReadInput()
    {
        if (Keyboard.current == null) return 0f;

        if (objectType == ObjectType.Wall)
        {
            if (Keyboard.current.wKey.isPressed) return 1f;
            if (Keyboard.current.sKey.isPressed) return -1f;
        }
        else
        {
            if (Keyboard.current.aKey.isPressed) return -1f;
            if (Keyboard.current.dKey.isPressed) return 1f;
        }

        return 0f;
    }
}