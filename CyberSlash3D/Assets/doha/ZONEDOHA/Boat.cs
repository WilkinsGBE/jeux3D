using UnityEngine;

public class Boat : MonoBehaviour
{
    public ObjectiveManager objectiveManager;
    public GameObject victoryPanel;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("BOAT TOUCHÉ");

            if (objectiveManager.AllSystemsActivated() && objectiveManager.hasBoatKey)
            {
                Debug.Log("VICTOIRE !");

                if (victoryPanel != null)
                    victoryPanel.SetActive(true);

                Time.timeScale = 0f;
            }
            else
            {
                Debug.Log("Objectifs incomplets !");
            }
        }
    }
}
