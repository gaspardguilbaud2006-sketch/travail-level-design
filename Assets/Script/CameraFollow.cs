using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    private float offsetX;

    void Start()
    {
        offsetX = transform.position.x - player.position.x;
    }

    void LateUpdate()
    {
        if (player == null) return;

        float newX = player.position.x + offsetX;

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
