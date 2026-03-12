using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class dragOnClick : MonoBehaviour
{
    [Header("Follow Settings")]
    public float followSpeed = 10f;

    [Header("Throw Settings")]
    [Range(1f, 10f)]
    public float throwForceMultiplier = 3f;
    public float maxThrowSpeed = 20f;

    [Header("Player Reference")]
    public Rigidbody2D playerRb;
    public Player_script playerScript;

    private bool isDragging = false;
    private Vector3 offset;

    private Camera mainCamera;
    private Collider2D objectCollider;
    private Rigidbody2D rb;

    private float originalGravity;

    // Les deux dernières positions enregistrées en FixedUpdate
    private Vector3 prevPosition;
    private Vector3 lastPosition;
    private float prevTime;
    private float lastTime;

    // Vélocité calculée en continu pendant le drag
    private Vector2 lastComputedVelocity = Vector2.zero;

    void Start()
    {
        mainCamera = Camera.main;
        objectCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;

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

        // Début du drag
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (objectCollider.OverlapPoint(mouseWorldPos))
            {
                isDragging = true;
                offset = transform.position - mouseWorldPos;

                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;
                lastComputedVelocity = Vector2.zero;

                // Initialise les deux slots avec la position actuelle
                prevPosition = transform.position;
                lastPosition = transform.position;
                prevTime = Time.fixedTime;
                lastTime = Time.fixedTime;
            }
        }

        // Fin du drag
        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
            StopDragging();

        // Déplacement pendant le drag (dans Update pour la réactivité souris)
        if (isDragging)
        {
            Vector3 targetPosition = mouseWorldPos + offset;

            if (playerRb != null && playerRb.linearVelocity.x > 0f)
                targetPosition.x += playerRb.linearVelocity.x * Time.deltaTime;

            Vector3 direction = targetPosition - transform.position;
            Vector3 smoothPosition = transform.position + direction * Mathf.Clamp01(followSpeed * Time.deltaTime);
            rb.MovePosition(smoothPosition);
        }
    }

    void FixedUpdate()
    {
        if (!isDragging) return;

        // Décale : l'ancien "last" devient "prev", et on enregistre la nouvelle position
        prevPosition = lastPosition;
        prevTime = lastTime;

        lastPosition = transform.position;
        lastTime = Time.fixedTime;

        // Calcule la vélocité entre les deux dernières frames physiques
        float deltaTime = lastTime - prevTime;
        if (deltaTime > 0f)
        {
            Vector2 velocity = ((Vector2)(lastPosition - prevPosition) / deltaTime) * throwForceMultiplier;
            lastComputedVelocity = Vector2.ClampMagnitude(velocity, maxThrowSpeed);
        }
    }

    void StopDragging()
    {
        isDragging = false;
        rb.gravityScale = originalGravity;

        // Applique directement la dernière vélocité calculée dans FixedUpdate
        rb.linearVelocity = lastComputedVelocity;
    }
}