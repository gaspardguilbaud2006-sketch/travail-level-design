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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        objectCollider = GetComponent<Collider2D>();

        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        if (player != null)
            playerCollider = player.GetComponent<Collider2D>();
        else
            Debug.LogWarning("Player non assigné sur " + gameObject.name);
    }

    void FixedUpdate()
    {
        float input = ReadInput();

        // Détection collisions player
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

            // Bloquer si le player est coincé entre mur et sol/plafond
            if (playerTouchingCeiling && input > 0f) input = 0f;
            if (playerTouchingGround && input < 0f) input = 0f;
        }

        // Appliquer le mouvement sur le bon axe
        Vector2 force = objectType == ObjectType.Wall
            ? new Vector2(0f, input)     // Mur : Z/S → axe Y
            : new Vector2(input, 0f);    // Plateforme : Q/D → axe X

        if (input != 0f)
        {
            rb.AddForce(force * acceleration);
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

    float ReadInput()
    {
        if (Keyboard.current == null) return 0f;

        if (objectType == ObjectType.Wall)
        {
            // Z = haut, S = bas
            if (Keyboard.current.zKey.isPressed) return 1f;
            if (Keyboard.current.sKey.isPressed) return -1f;
        }
        else
        {
            // Q = gauche, D = droite
            if (Keyboard.current.qKey.isPressed) return -1f;
            if (Keyboard.current.dKey.isPressed) return 1f;
        }

        return 0f;
    }
}