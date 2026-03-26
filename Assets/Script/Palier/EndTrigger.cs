using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [Header("Objet à activer")]
    public GameObject objectToActivate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Active le GameObject (UI, écran de fin, etc.)
            if (objectToActivate != null)
                objectToActivate.SetActive(true);

            // Stop le score
            ScoreManager.stopScore = true;
        }
    }
}