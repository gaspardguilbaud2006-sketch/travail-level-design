using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject ballPrefab;
    public float spawnRate = 2f;

    [Header("References")]
    public Player_script playerScript;

    private float timer = 0f;

    void Update()
    {
        if (playerScript != null && playerScript.GameOver) return;

        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            timer = 0f;
            if (ballPrefab != null)
                Instantiate(ballPrefab, transform.position, Quaternion.identity);
        }
    }

    // Affiche le point de spawn dans l'éditeur
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}
