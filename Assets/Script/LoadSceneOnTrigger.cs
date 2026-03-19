using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTrigger : MonoBehaviour
{
    [Header("Scene à charger")]
    public string sceneName;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie que c'est bien le joueur
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            LoadScene();
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}