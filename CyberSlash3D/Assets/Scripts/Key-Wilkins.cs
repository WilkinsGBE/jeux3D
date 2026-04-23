using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    public static int keysCollected = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            keysCollected++;
            Debug.Log("Clé ramassée! Total: " + keysCollected + " / 2");
            gameObject.SetActive(false);

            if (keysCollected >= 2)
                Debug.Log("2 clés trouvées — porte ouverte!");
        }
    }
}