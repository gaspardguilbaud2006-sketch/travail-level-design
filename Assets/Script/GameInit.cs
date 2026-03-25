using UnityEngine;

public class GameInit : MonoBehaviour
{
    void Awake()
    {
        if (!GameStartTracker.hasStarted)
        {
            CheckpointManager.ClearCheckpoint();
            ScoreManager.ResetScore();
            GameStartTracker.hasStarted = true;
        }
    }
}