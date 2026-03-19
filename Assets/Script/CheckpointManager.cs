using UnityEngine;

public static class CheckpointManager
{
    private const string KEY_X = "checkpoint_x";
    private const string KEY_Y = "checkpoint_y";
    private const string KEY_SET = "checkpoint_set";

    public static void SaveCheckpoint(Vector3 position)
    {
        PlayerPrefs.SetFloat(KEY_X, position.x);
        PlayerPrefs.SetFloat(KEY_Y, position.y);
        PlayerPrefs.SetInt(KEY_SET, 1);
        PlayerPrefs.Save();
    }

    public static bool HasCheckpoint()
    {
        return PlayerPrefs.GetInt(KEY_SET, 0) == 1;
    }

    public static Vector3 LoadCheckpoint()
    {
        float x = PlayerPrefs.GetFloat(KEY_X, 0f);
        float y = PlayerPrefs.GetFloat(KEY_Y, 0f);
        return new Vector3(x, y, 0f);
    }

    public static void ClearCheckpoint()
    {
        PlayerPrefs.DeleteKey(KEY_X);
        PlayerPrefs.DeleteKey(KEY_Y);
        PlayerPrefs.DeleteKey(KEY_SET);
    }
}