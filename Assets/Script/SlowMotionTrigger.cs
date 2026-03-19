using UnityEngine;
using UnityEngine.InputSystem;

public class SlowMotionTrigger : MonoBehaviour
{
    [Header("Réglages")]
    public float slowSpeed = 0f;
    public float lerpSpeed = 2f;

    [Header("Input")]
    public Key continueKey = Key.Space;

    [Header("UI / Objet à activer")]
    public GameObject objectToActivate;

    private bool slowingDown = false;
    private bool speedingUp = false;
    private bool hasTriggered = false;

    void Update()
    {
        bool keyPressed = Keyboard.current != null && Keyboard.current[continueKey].wasPressedThisFrame;

        if (slowingDown && !speedingUp)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, slowSpeed, Time.unscaledDeltaTime * lerpSpeed);

            if (!hasTriggered)
            {
                hasTriggered = true;
                if (objectToActivate != null)
                    objectToActivate.SetActive(true);
            }
        }

        if (hasTriggered && keyPressed)
        {
            slowingDown = false;
            speedingUp = true;

            if (objectToActivate != null)
                objectToActivate.SetActive(false);
        }

        if (speedingUp)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Time.unscaledDeltaTime * lerpSpeed);

            if (Time.timeScale >= 0.99f)
            {
                Time.timeScale = 1f;
                speedingUp = false;

                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            slowingDown = true;
        }
    }
}