using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class dragOnClick : MonoBehaviour
{
    [Header("Follow Settings")]
    public float followSpeed = 10f;

    [Header("Momentum Settings")]
    [Range(0f, 1f)]
    public float momentumFactor = 0.5f;

    [Header("Player Reference")]
    public Rigidbody2D playerRb; 
    public Player_script playerScript;

    private bool isDragging = false;
    private Vector3 offset;

    private Camera mainCamera;
    private Collider2D objectCollider;
    private Rigidbody2D rb;

    private float originalGravity;
    private Vector3 lastPosition;
    private Vector3 currentVelocity;

    void Start()
    {
        mainCamera = Camera.main;
        objectCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;

        if (playerRb == null)
        {
            Debug.LogWarning("Player Rigidbody2D non assigné !");
        }

        if (playerScript == null)
        {
            Debug.LogWarning("Player_script non assigné !");
        }
    }

    void Update()
    {
        if (Mouse.current == null) return;

        if (playerScript != null && playerScript.GameOver)
        {
            if (isDragging)
            {
                isDragging = false;
                rb.gravityScale = originalGravity;
                rb.linearVelocity = currentVelocity * momentumFactor;
            }
            return;
        }

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (objectCollider.OverlapPoint(mouseWorldPos))
            {
                isDragging = true;
                offset = transform.position - mouseWorldPos;

                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;
                lastPosition = transform.position;
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            rb.gravityScale = originalGravity;
            rb.linearVelocity = currentVelocity * momentumFactor;
        }

        // Drag
        if (isDragging)
        {
            Vector3 targetPosition = mouseWorldPos + offset;

            float playerSpeedX = 0f;
            if (playerRb != null && playerRb.linearVelocity.x > 0f)
            {
                playerSpeedX = playerRb.linearVelocity.x * Time.deltaTime;
            }

            targetPosition.x += playerSpeedX;

            Vector3 direction = targetPosition - transform.position;
            Vector3 smoothPosition = transform.position + direction * Mathf.Clamp01(followSpeed * Time.deltaTime);

            rb.MovePosition(smoothPosition);

            currentVelocity = (smoothPosition - lastPosition) / Time.deltaTime;
            lastPosition = smoothPosition;
        }
    }
}

