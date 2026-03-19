using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    void Start()
    {
        // Au chargement de la scène, repositionne le joueur si un checkpoint existe
        if (CheckpointManager.HasCheckpoint())
        {
            transform.position = CheckpointManager.LoadCheckpoint();
        }
    }
}