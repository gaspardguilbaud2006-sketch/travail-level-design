using UnityEngine;

public class ScoreTierManager : MonoBehaviour
{
    [Header("Paliers de score (ordre croissant)")]
    public float[] scoreThresholds;

    [Header("Objets associés aux paliers")]
    public GameObject[] tierObjects;

    void Update()
    {
        float currentScore = ScoreManager.GetScore();

        int currentTier = 0;

        // Détermine le palier actuel
        for (int i = 0; i < scoreThresholds.Length; i++)
        {
            if (currentScore >= scoreThresholds[i])
            {
                currentTier = i + 1;
            }
        }

        // Sécurité (évite dépassement tableau)
        currentTier = Mathf.Clamp(currentTier, 0, tierObjects.Length - 1);

        // Active seulement le bon GameObject
        for (int i = 0; i < tierObjects.Length; i++)
        {
            if (tierObjects[i] != null)
                tierObjects[i].SetActive(i == currentTier);
        }
    }
}