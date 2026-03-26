using UnityEngine;

public class ActivateOnPlayerTrigger : MonoBehaviour
{
    [Header("Objet à activer")]
    public GameObject objectToActivate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }
        }
    }
}