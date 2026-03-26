using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Références")]
    public Player_script player;
    public TextMeshProUGUI scoreText;

    [Header("Paramètres")]
    public float scoreMultiplier = 10f;

    [Header("Pénalité")]
    public float negativeSpeedPenalty = 150f;

    private static float score = 0f;
    public static bool stopScore = false;

    private bool wasNegative = false;

    void Start()
    {
        // Score à zéro au début du niveau
        ResetScore();
    }

    void Update()
    {
        if (stopScore) return;

        // Gain de score normal
        if (player.CurrentSpeed > 0f)
            score += player.CurrentSpeed * scoreMultiplier * Time.deltaTime;

        // Pénalité si vitesse négative
        if (player.CurrentSpeed < 0f)
        {
            if (!wasNegative)
            {
                score -= negativeSpeedPenalty;
                wasNegative = true;
            }
        }
        else
        {
            wasNegative = false;
        }

        // Empêche score négatif
        score = Mathf.Max(0f, score);

        // Mise à jour de l'UI
        if (scoreText != null)
            scoreText.text = "Score : " + Mathf.FloorToInt(score);
    }

    /// <summary>
    /// Réinitialise le score à zéro (début de niveau)
    /// </summary>
    public static void ResetScore()
    {
        score = 0f;
        stopScore = false;
    }

    /// <summary>
    /// Récupère le score actuel
    /// </summary>
    public static float GetScore()
    {
        return score;
    }

    /// <summary>
    /// Définit le score à une valeur précise (restauration checkpoint)
    /// </summary>
    public static void SetScore(float value)
    {
        score = value;
    }
}