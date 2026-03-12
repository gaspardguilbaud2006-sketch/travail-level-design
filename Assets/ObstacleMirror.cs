using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObstacleMirror : MonoBehaviour
{
    [Header("Target à suivre")]
    public Rigidbody2D target;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        if (target == null)
            Debug.LogWarning("Aucune cible assignée sur " + gameObject.name);
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // Copie exacte de la vélocité de la cible
        rb.linearVelocity = target.linearVelocity;
    }
}