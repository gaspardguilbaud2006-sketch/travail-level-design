using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleGameObject : MonoBehaviour
{
    public GameObject objectToEnable;

    void Update()
    {
        // Vérifie si la touche espace est pressée (nouveau système)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true);
            }

            gameObject.SetActive(false);
        }
    }
}