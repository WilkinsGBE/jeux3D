using UnityEngine;

public class BossPush : MonoBehaviour
{
    public float pushStrength = 10f;
    public float minDistance = 1.2f; // distance from boss center

    private void OnTriggerStay(Collider other)
    {
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc == null) return;

        Vector3 bossPos = transform.position;
        Vector3 playerPos = other.transform.position;

        // Direction from boss to player (flat)
        Vector3 dir = playerPos - bossPos;
        dir.y = 0f;

        float distance = dir.magnitude;

        if (distance < 0.001f)
            return;

        // Normalize direction
        dir /= distance;

        // Push strength increases when closer
        float pushForce = pushStrength * (1f - Mathf.Clamp01(distance / minDistance));

        // Apply push
        cc.Move(dir * pushForce * Time.deltaTime);
    }
}