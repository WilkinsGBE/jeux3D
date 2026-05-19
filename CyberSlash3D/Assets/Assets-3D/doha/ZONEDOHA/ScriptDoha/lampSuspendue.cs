using UnityEngine;

public class LampSuspendue : MonoBehaviour
{
    [Header("Balancement")]
    public float forceBalancement = 3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        Vector3 direction = transform.position - collision.contacts[0].point;
        direction.y = 0f;
        direction.Normalize();

        rb.AddForce(direction * forceBalancement, ForceMode.Impulse);
    }
}