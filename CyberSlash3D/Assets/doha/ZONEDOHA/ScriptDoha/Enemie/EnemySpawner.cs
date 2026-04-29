using UnityEngine;

// Ce script gère le spawn des ennemis dans la scène
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab; // Prefab de l'ennemi à créer

    [Header("Spawn Points")]
    public Transform spawnPoint1;   // Point de spawn 1
    public Transform spawnPoint2;   // Point de spawn 2

    // ===================== SPAWN ENEMIES =====================
    //public void SpawnEnemies(int amount)
    //{
    //    // Boucle pour créer plusieurs ennemis
    //    for (int i = 0; i < amount; i++)
    //    {
    //        // Choisit un point de spawn aléatoire
    //        Transform chosenPoint = Random.value > 0.5f 
    //            ? spawnPoint1 
    //            : spawnPoint2;

    //        // Crée l’ennemi à la position du point choisi
    //        Instantiate(enemyPrefab, chosenPoint.position, chosenPoint.rotation);
    //    }

    //    Debug.Log("👾 Ennemis spawnés : " + amount);
    //}
}