using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Références")]
    public Player_script player;
    public TextMeshProUGUI scoreText;

    [Header("Paramètres")]
    public float scoreMultiplier = 10f;

    // Variable statique : survit au rechargement de scène mais pas au lancement du jeu
    private static float score = 0f;

    void Update()
    {
        score += player.CurrentSpeed * scoreMultiplier * Time.deltaTime;
        score = Mathf.Max(0f, score);

        scoreText.text = "Score : " + Mathf.FloorToInt(score);
    }

    public static void ResetScore()
    {
        score = 0f;
    }
}