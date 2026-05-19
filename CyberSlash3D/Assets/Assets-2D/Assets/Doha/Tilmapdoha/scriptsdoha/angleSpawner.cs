using UnityEngine;
//gere l'angel spawn
public class angleSpawner : MonoBehaviour
{
    public GameObject angelPrefab;

    public Transform spawnPoint;

    // Fonction qui crée l'ange et retourne l'objet créé
    public GameObject SpawnAngel()
    {
        Debug.Log("SpawnAngel called");

        // Vérifie que le prefab est bien assigné
        if (angelPrefab == null)
        {
            Debug.LogError("angelPrefab is NULL");
            return null;
        }

        // Vérifie que le point de spawn est bien assigné
        if (spawnPoint == null)
        {
            Debug.LogError("spawnPoint is NULL");
            return null;
        }

        // Affiche la position où l'ange va apparaître
        Debug.Log("Spawning angel at: " + spawnPoint.position);

        // Crée l'ange dans la scène
        GameObject angel = Instantiate(
            angelPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        Debug.Log("Angel object created: " + angel.name);

        return angel;
    }
}