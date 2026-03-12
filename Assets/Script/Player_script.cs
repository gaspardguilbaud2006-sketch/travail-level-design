using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_script : MonoBehaviour
{
    [Header("Movement")]
    public float startSpeed = 5f;
    public float maxSpeed = 20f;
    public float acceleration = 0.5f;
    public float lerpSpeed = 2f;

    [Header("Wall Bounce")]
    public float bounceBackSpeed = 8f;      // Vitesse de recul horizontal
    public float bounceUpForce = 6f;        // Force du saut arrière
    public float bounceDuration = 0.4f;     // Durée du recul avant de repartir

    [Header("Game State")]
    public bool GameOver = false;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private float targetSpeed;

    private bool isBouncing = false;
    private float bounceTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetSpeed = startSpeed;
    }

    void Update()
    {
        if (!GameOver)
        {
            if (isBouncing)
            {
                bounceTimer -= Time.deltaTime;
                if (bounceTimer <= 0f)
                {
                    isBouncing = false;
                    // Réinitialise la vitesse cible pour repartir en douceur
                    targetSpeed = startSpeed;
                }
            }
            else
            {
                MovePlayer();
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void MovePlayer()
    {
        targetSpeed += acceleration * Time.deltaTime;
        targetSpeed = Mathf.Clamp(targetSpeed, startSpeed, maxSpeed);

        float currentSpeed = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, lerpSpeed * Time.deltaTime);
        float yVelocity = rb.linearVelocity.y;

        if (isGrounded)
            yVelocity = 0f;

        rb.linearVelocity = new Vector2(currentSpeed, yVelocity);
    }

    void TriggerWallBounce()
    {
        isBouncing = true;
        bounceTimer = bounceDuration;

        // Saut en arrière : recul horizontal + impulsion vers le haut
        rb.linearVelocity = new Vector2(-bounceBackSpeed, bounceUpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Détection mur par normale de contact (surface verticale)
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Si la normale est majoritairement horizontale  c'est un mur
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
}