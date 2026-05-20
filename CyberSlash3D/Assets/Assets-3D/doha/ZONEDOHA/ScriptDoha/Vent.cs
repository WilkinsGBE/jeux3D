using UnityEngine;

public class Vent : MonoBehaviour
{
    [Header("Vent")]
    public Vector3 direction = Vector3.forward;
    public float force = 10f;

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Essaie Rigidbody d'abord
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction.normalized * force, ForceMode.Force);
            return;
        }

        // Sinon utilise CharacterController
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.Move(direction.normalized * force * Time.deltaTime);
            Debug.Log("Vent appliqué via CharacterController !");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
        Gizmos.DrawRay(transform.position, direction.normalized * 4f);
    }
}