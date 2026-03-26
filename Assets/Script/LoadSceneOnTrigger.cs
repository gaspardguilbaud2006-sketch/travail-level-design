using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LoadSceneOnTrigger : MonoBehaviour
{
    [Header("Scene à charger")]
    public string sceneName;

    [Header("Touche d'activation")]
    public Key keyToPress = Key.Space;

    private bool hasTriggered = false;

    void Update()
    {
        // Vérifie si la touche est pressée
        if (!hasTriggered && Keyboard.current[keyToPress].wasPressedThisFrame)
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