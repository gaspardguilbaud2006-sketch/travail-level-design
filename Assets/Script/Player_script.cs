using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_script : MonoBehaviour
{
    [Header("Movement")]
    public float startSpeed = 5f;
    public float maxSpeed = 20f;
    public float acceleration = 0.5f;
    public float lerpSpeed = 2f;

    [Header("Game State")]
    public bool GameOver = false;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    private float targetSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetSpeed = startSpeed;
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

        targetSpeed += acceleration * Time.deltaTime;
        targetSpeed = Mathf.Clamp(targetSpeed, startSpeed, maxSpeed);

        float currentSpeed = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, lerpSpeed * Time.deltaTime);

        float yVelocity = rb.linearVelocity.y;

        if (isGrounded)
        {
            yVelocity = 0f;
        }

        rb.linearVelocity = new Vector2(currentSpeed, yVelocity);
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
}