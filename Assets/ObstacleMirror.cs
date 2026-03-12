using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObstacleMirror : MonoBehaviour
{
    [Header("Target à suivre")]
    public Rigidbody2D target;

    private Rigidbody2D rb;
    private Vector2 lastTargetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        if (target == null)
        {
            Debug.LogWarning("Aucune cible assignée sur " + gameObject.name);
            return;
        }

        lastTargetPosition = target.position;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // Vecteur de déplacement de la cible depuis la dernière frame physique
        Vector2 delta = target.position - lastTargetPosition;
        lastTargetPosition = target.position;

        // Applique le même vecteur de déplacement à l'obstacle
        rb.MovePosition(rb.position + delta);
    }
}