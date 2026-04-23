using UnityEngine;
using System.Collections;

public class WelcomeZone : MonoBehaviour
{
    public GameObject welcomePanel;
    public Vector3 gizmoSize = new Vector3(3f, 2f, 3f);
    private bool triggered = false;

    void Start()
    {
        Debug.Log("WelcomeZone START — welcomePanel est null? " + (welcomePanel == null));
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter détecté — objet: " + other.gameObject.name + " | Tag: " + other.tag);

        if (!triggered && other.CompareTag("Player"))
        {
            Debug.Log("Player détecté — lancement du message");
            triggered = true;
            StartCoroutine(ShowMessage());
        }
    }

    IEnumerator ShowMessage()
    {
        Debug.Log("ShowMessage — activation du panel");
        welcomePanel.SetActive(true);
        yield return new WaitForSeconds(4f);
        welcomePanel.SetActive(false);
        Debug.Log("ShowMessage — panel désactivé");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.3f);
        Gizmos.DrawCube(transform.position, gizmoSize);
        Gizmos.color = new Color(0f, 1f, 0.5f, 1f);
        Gizmos.DrawWireCube(transform.position, gizmoSize);
    }
}