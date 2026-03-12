using UnityEngine;
using TMPro;

public class SpeedCounter : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D playerRb;
    public TMP_Text speedText;

    void Update()
    {
        if (playerRb == null || speedText == null) return;

        int speed = Mathf.RoundToInt(playerRb.linearVelocity.x);
        speedText.text = speed.ToString();
    }
}
