using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class SwitchButton : MonoBehaviour
{
    [Header("State")]
    public int currentState = 1;

    [Header("State 1 - Actives")]
    public GameObject[] state1ActiveObjects;

    [Header("State 1 - Inactives")]
    public GameObject[] state1InactiveObjects;

    [Header("Visual Feedback")]
    public SpriteRenderer buttonSpriteRenderer;
    public Sprite state1Sprite;
    public Sprite state2Sprite;

    [Header("Player Reference")]
    public Player_script playerScript;

    private Collider2D buttonCollider;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        buttonCollider = GetComponent<Collider2D>();

        if (buttonSpriteRenderer == null)
            buttonSpriteRenderer = GetComponent<SpriteRenderer>();

        if (playerScript == null)
            Debug.LogWarning("Player_script non assigné !");

        ApplyState();
    }

    void Update()
    {
        if (Mouse.current == null) return;

        if (playerScript != null && playerScript.GameOver) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = 0f;

            if (buttonCollider.OverlapPoint(mouseWorldPos))
            {
                ToggleState();
            }
        }
    }
    void ToggleState()
    {
        currentState = (currentState == 1) ? 2 : 1;
        ApplyState();
    }

    void ApplyState()
    {
        bool isState1 = currentState == 1;

        // State 1 objects : actifs en état 1, inactifs en état 2
        foreach (GameObject obj in state1ActiveObjects)
            if (obj != null) obj.SetActive(isState1);

        // State 1 inactive objects : inactifs en état 1, actifs en état 2
        foreach (GameObject obj in state1InactiveObjects)
            if (obj != null) obj.SetActive(!isState1);

        // Feedback visuel
        if (buttonSpriteRenderer != null)
        {
            if (isState1 && state1Sprite != null)
                buttonSpriteRenderer.sprite = state1Sprite;
            else if (!isState1 && state2Sprite != null)
                buttonSpriteRenderer.sprite = state2Sprite;
        }
    }
}