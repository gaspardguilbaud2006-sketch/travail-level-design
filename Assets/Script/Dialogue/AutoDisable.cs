using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    [Header("Temps avant désactivation")]
    public float delay = 2f;

    void OnEnable()
    {
        // Lance le timer dès que l'objet est activé
        Invoke(nameof(DisableObject), delay);
    }

    void DisableObject()
    {
        gameObject.SetActive(false);
    }
}