using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_script : MonoBehaviour
{
    [HideInInspector] public float CurrentSpeed;

    [Header("Movement")]
    public float startSpeed = 5f;
    public float maxSpeed = 20f;
    public float acceleration = 0.5f;
    public float lerpSpeed = 2f;

    [Header("Wall Bounce")]
    public float bounceBackSpeed = 8f;
    public float bounceUpForce = 6f;
    public float bounceDuration = 0.4f;

    [Header("Game State")]
    public bool GameOver = false;

    [HideInInspector] public bool isRespawning = false; // Bloque le mouvement temporairement

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private float targetSpeed;

    private bool isBouncing = false;
    private float bounceTimer = 0f;

    // INITIALISATION
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        targetSpeed = startSpeed;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        // Si le joueur est en respawn ou game over, bloquer le mouvement
        if (GameOver || isRespawning)
        {
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isBouncing)
        {
            bounceTimer -= Time.deltaTime;
            if (bounceTimer <= 0f)
            {
                isBouncing = false;
                targetSpeed = startSpeed;
            }
        }
        else
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        if (isRespawning) return; // bloque le mouvement pendant respawn

        targetSpeed += acceleration * Time.deltaTime;
        targetSpeed = Mathf.Clamp(targetSpeed, startSpeed, maxSpeed);

        CurrentSpeed = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, lerpSpeed * Time.deltaTime);

        float yVelocity = rb.linearVelocity.y;
        if (isGrounded)
            yVelocity = 0f;

        rb.linearVelocity = new Vector2(CurrentSpeed, yVelocity);
    }

    void TriggerWallBounce()
    {
        isBouncing = true;
        bounceTimer = bounceDuration;

        rb.linearVelocity = new Vector2(-bounceBackSpeed, bounceUpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > 0.7f && !isBouncing)
            {
                TriggerWallBounce();
                break;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ne rien téléporter ici : PlayerRespawn gère le placement
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        targetSpeed = startSpeed;
        isBouncing = false;
        bounceTimer = 0f;
        GameOver = false;
    }
}