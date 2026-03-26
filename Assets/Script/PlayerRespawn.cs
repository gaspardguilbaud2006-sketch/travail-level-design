using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Spawn initial")]
    public Transform spawnPoint;

    [Header("Paramètres")]
    public float respawnScorePenalty = 600f;

    private Player_script playerScript;
    private Rigidbody2D rb;

    void Awake()
    {
        playerScript = GetComponent<Player_script>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Vérifie si la scène actuelle a un checkpoint
        Vector3 targetPos;
        float checkpointScore = 0f;

        if (CheckpointManager.HasCheckpoint())
        {
            targetPos = CheckpointManager.LoadCheckpointPosition();
            checkpointScore = CheckpointManager.LoadCheckpointScore();
        }
        else
        {
            // Pas de checkpoint pour cette scène : spawn initial
            targetPos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
            checkpointScore = 0f;
        }

        TeleportPlayer(targetPos, checkpointScore);
    }

    public void RespawnAfterDeath()
    {
        Vector3 targetPos;
        float checkpointScore = 0f;

        if (CheckpointManager.HasCheckpoint())
        {
            targetPos = CheckpointManager.LoadCheckpointPosition();
            checkpointScore = CheckpointManager.LoadCheckpointScore();

            // Appliquer pénalité
            checkpointScore -= respawnScorePenalty;
            checkpointScore = Mathf.Max(0f, checkpointScore);

            // Réenregistrer le checkpoint pénalisé pour cette scène
            CheckpointManager.SaveCheckpoint(targetPos, checkpointScore);
        }
        else
        {
            targetPos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
            checkpointScore = 0f;
        }

        TeleportPlayer(targetPos, checkpointScore);
    }

    private void TeleportPlayer(Vector3 targetPos, float score)
    {
        if (playerScript != null)
            playerScript.isRespawning = true;

        if (rb != null)
        {
            rb.position = targetPos;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            transform.position = targetPos;
        }

        ScoreManager.SetScore(score);

        if (playerScript != null)
            playerScript.isRespawning = false;

        Debug.Log("Respawn vers : " + targetPos + " | Score : " + score);
    }
}