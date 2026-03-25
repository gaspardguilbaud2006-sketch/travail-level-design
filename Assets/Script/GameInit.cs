using UnityEngine;

public class GameInit : MonoBehaviour
{
    void Awake()
    {
        if (!GameStartTracker.hasStarted)
        {
            CheckpointManager.ClearCheckpoint();
            ScoreManager.ResetScore(); // 👈 déjà là ?
            GameStartTracker.hasStarted = true;
        }
    }
}