using UnityEngine;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite flagInactive; // drapeau blanc
    public Sprite flagActive;   // drapeau rouge

    [Header("UI")]
    public GameObject checkpointText; // objet TextMeshPro enfant

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (checkpointText != null)
            checkpointText.SetActive(false); // caché au départ
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointManager.SaveCheckpoint(transform.position);

            // Changer le sprite en rouge
            if (flagActive != null)
                spriteRenderer.sprite = flagActive;

            // Afficher le texte
            if (checkpointText != null)
                checkpointText.SetActive(true);
        }
    }
}