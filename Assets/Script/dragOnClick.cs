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

    [Header("Throw Settings")]
    public int velocityBufferSize = 3;        // Moins de frames = plus réactif
    [Range(1f, 5f)]
    public float throwForceMultiplier = 2f;   // Amplifie l'inertie au lancer

    [Header("Player Reference")]
    public Rigidbody2D playerRb;
    public Player_script playerScript;

    private bool isDragging = false;
    private Vector3 offset;

    private Camera mainCamera;
    private Collider2D objectCollider;
    private Rigidbody2D rb;

    private float originalGravity;

    private Vector3 lastMouseWorldPos;
    private Vector3 currentVelocity;
    private Vector3[] velocityBuffer;
    private int velocityBufferIndex = 0;

    void Start()
    {
        mainCamera = Camera.main;
        objectCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        velocityBuffer = new Vector3[velocityBufferSize];

        if (playerRb == null)
            Debug.LogWarning("Player Rigidbody2D non assigné !");
        if (playerScript == null)
            Debug.LogWarning("Player_script non assigné !");
    }

    void Update()
    {
        if (Mouse.current == null) return;

        if (playerScript != null && playerScript.GameOver)
        {
            if (isDragging) StopDragging();
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
                lastMouseWorldPos = mouseWorldPos;
                velocityBufferIndex = 0;

                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;

                velocityBuffer = new Vector3[velocityBufferSize];
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            StopDragging();
        }

        if (isDragging)
        {
            Vector3 targetPosition = mouseWorldPos + offset;

            if (playerRb != null && playerRb.linearVelocity.x > 0f)
                targetPosition.x += playerRb.linearVelocity.x * Time.deltaTime;

            Vector3 direction = targetPosition - transform.position;
            Vector3 smoothPosition = transform.position + direction * Mathf.Clamp01(followSpeed * Time.deltaTime);
            rb.MovePosition(smoothPosition);

            // Stocke uniquement les N dernières frames (circulaire)
            Vector3 frameVelocity = (mouseWorldPos - lastMouseWorldPos) / Time.deltaTime;
            velocityBuffer[velocityBufferIndex % velocityBufferSize] = frameVelocity;
            velocityBufferIndex++;

            // Moyenne pondérée : les frames récentes ont plus de poids
            currentVelocity = Vector3.zero;
            float totalWeight = 0f;
            for (int i = 0; i < velocityBufferSize; i++)
            {
                float weight = i + 1f; // Frame la plus récente = poids le plus élevé
                currentVelocity += velocityBuffer[i] * weight;
                totalWeight += weight;
            }
            currentVelocity /= totalWeight;

            lastMouseWorldPos = mouseWorldPos;
        }
    }

    void StopDragging()
    {
        isDragging = false;
        rb.gravityScale = originalGravity;
        rb.linearVelocity = currentVelocity * momentumFactor * throwForceMultiplier;
    }
}