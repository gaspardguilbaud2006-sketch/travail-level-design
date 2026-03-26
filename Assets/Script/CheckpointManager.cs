using UnityEngine;
using UnityEngine.SceneManagement;

public static class CheckpointManager
{
    private const string KEY_X = "checkpoint_x";
    private const string KEY_Y = "checkpoint_y";
    private const string KEY_SCORE = "checkpoint_score";
    private const string KEY_SET = "checkpoint_set";

    private static string ScenePrefix => SceneManager.GetActiveScene().name + "_";

    public static void SaveCheckpoint(Vector3 position, float score)
    {
        PlayerPrefs.SetFloat(ScenePrefix + KEY_X, position.x);
        PlayerPrefs.SetFloat(ScenePrefix + KEY_Y, position.y);
        PlayerPrefs.SetFloat(ScenePrefix + KEY_SCORE, score);
        PlayerPrefs.SetInt(ScenePrefix + KEY_SET, 1);
        PlayerPrefs.Save();
    }

    public static bool HasCheckpoint()
    {
        return PlayerPrefs.GetInt(ScenePrefix + KEY_SET, 0) == 1;
    }

    public static Vector3 LoadCheckpointPosition()
    {
        float x = PlayerPrefs.GetFloat(ScenePrefix + KEY_X, 0f);
        float y = PlayerPrefs.GetFloat(ScenePrefix + KEY_Y, 0f);
        return new Vector3(x, y, 0f);
    }

    public static float LoadCheckpointScore()
    {
        return PlayerPrefs.GetFloat(ScenePrefix + KEY_SCORE, 0f);
    }

    public static void ClearCheckpoint()
    {
        PlayerPrefs.DeleteKey(ScenePrefix + KEY_X);
        PlayerPrefs.DeleteKey(ScenePrefix + KEY_Y);
        PlayerPrefs.DeleteKey(ScenePrefix + KEY_SCORE);
        PlayerPrefs.DeleteKey(ScenePrefix + KEY_SET);
    }
}