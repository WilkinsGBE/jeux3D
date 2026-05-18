using UnityEngine;

public class EnemyPush : MonoBehaviour
{
    public float pushStrength = 12f;
    public float minDistance = 1.5f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc == null) return;

        Vector3 enemyPos = transform.position;
        Vector3 playerPos = other.transform.position;

        Vector3 dir = playerPos - enemyPos;
        dir.y = 0f;

        float distance = dir.magnitude;

        if (distance < 0.001f)
            return;

        dir /= distance;

        // stronger push when too close
        float pushForce = pushStrength * (1f - Mathf.Clamp01(distance / minDistance));

        cc.Move(dir * pushForce * Time.deltaTime);
    }
}