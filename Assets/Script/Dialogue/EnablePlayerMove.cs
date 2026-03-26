using UnityEngine;
using UnityEngine.InputSystem;

public class EnablePlayerMove : MonoBehaviour
{
    public Player_script playerScript;

    void Update()
    {
        // Nouveau système d'input
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (playerScript != null)
            {
                playerScript.canMove = true;
            }

            gameObject.SetActive(false);
        }
    }
}