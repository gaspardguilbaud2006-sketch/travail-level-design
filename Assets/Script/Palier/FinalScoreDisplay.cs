using UnityEngine;
using TMPro;

public class FinalScoreDisplay : MonoBehaviour
{
    [Header("Texte à mettre à jour")]
    public TextMeshProUGUI scoreText;

    void OnEnable()
    {
        if (scoreText != null)
        {
            // Récupère le score final depuis ScoreManager
            int finalScore = Mathf.FloorToInt(ScoreManager.GetScore());

            // Affiche uniquement le score
            scoreText.text = finalScore.ToString();
        }
    }
}