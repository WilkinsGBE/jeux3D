using UnityEngine;

public class CameraTargetFollow : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        transform.position = player.position;
    }
}