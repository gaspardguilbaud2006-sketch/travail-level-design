using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Références")]
    public Player_script player;
    public TextMeshProUGUI scoreText;

    [Header("Paramètres")]
    public float scoreMultiplier = 10f;

    private float score = 0f;
    private const string KEY_SCORE = "player_score";

    void Start()
    {
        // Recharge le score sauvegardé
        score = PlayerPrefs.GetFloat(KEY_SCORE, 0f);
    }

    void Update()
    {
        score += player.CurrentSpeed * scoreMultiplier * Time.deltaTime;
        score = Mathf.Max(0f, score);

        scoreText.text = "Score : " + Mathf.FloorToInt(score);

        // Sauvegarde en continu
        PlayerPrefs.SetFloat(KEY_SCORE, score);
    }

    public static void ResetScore()
    {
        PlayerPrefs.DeleteKey("player_score");
    }
}