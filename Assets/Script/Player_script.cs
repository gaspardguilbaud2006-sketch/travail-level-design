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
        if (!GameOver)
        {
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
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void MovePlayer()
    {
        targetSpeed += acceleration * Time.deltaTime;
        targetSpeed = Mathf.Clamp(targetSpeed, startSpeed, maxSpeed);

        // Remplace "float currentSpeed = ..." par :
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
        {
            isGrounded = true;
        }

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
        GameObject spawn = GameObject.Find("SpawnPoint");

        if (spawn != null)
            transform.position = spawn.transform.position;
        else
            transform.position = Vector3.zero;

        rb.linearVelocity = Vector2.zero;

        targetSpeed = startSpeed;
        isBouncing = false;
        bounceTimer = 0f;
        GameOver = false;

        transform.rotation = Quaternion.identity;
    }
}