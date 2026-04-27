using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    public GameObject enemyPrefab;

    public void SpawnEnemies(int amount, Transform spawnPoint)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    
}

