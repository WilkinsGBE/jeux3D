using UnityEngine;

public class Terminal : MonoBehaviour
{
    public ObjectiveManager objectiveManager;
    //public GameObject terminalPanel; // UI 
     public EnemySpawner spawner;  
    public int enemiesToSpawn = 5; 
        private bool activated = false;
        public Transform spawnPoint;

    void Start()
    {
        // terminalPanel.SetActive(false); // cache au début
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !activated)
        {
            activated = true;
            objectiveManager.ActivateSystem();

            //    terminalPanel.SetActive(true); 

            Debug.Log("Terminal activé !");
              if (spawner != null)
            {
spawner.SpawnEnemies(enemiesToSpawn, spawnPoint);
            }

            Debug.Log("Terminal activé ! + spawn ennemis");
        
        }
    }
}
