using UnityEngine;
using TMPro;
using System.Collections;
//gere la zone de l'angle
public class AngleZone : MonoBehaviour
{
    public Transform center;

    public float radius = 6f;

    public angleSpawner spawner;

    [Header("UI")]
    public GameObject messageUI;

    public TextMeshProUGUI messageText;

    [Header("Numbers")]
    public GameObject[] numbers;

    private bool activated = false;

    private void Start()
    {
        Debug.Log("AngleZone START");

        Debug.Log("Numbers assigned = " + (numbers != null ? numbers.Length : -1));

        // Vérification du tableau de nombres
        if (numbers == null)
            Debug.LogError("numbers ARRAY is NULL");
        else if (numbers.Length == 0)
            Debug.LogWarning("numbers ARRAY is EMPTY (drag objects in Inspector)");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger ENTER detected with: " + other.name);

        // Empêche de relancer la zone plusieurs fois
        if (activated)
        {
            Debug.Log("Already activated");
            return;
        }

        // Vérifie que c'est bien le joueur
        if (!other.CompareTag("Player"))
        {
            Debug.Log("Not player, tag = " + other.tag);
            return;
        }

        activated = true;
        Debug.Log("PLAYER ENTERED ANGLE ZONE");

        // Affiche le message UI
        if (messageUI == null)
        {
            Debug.LogError("messageUI is NULL");
        }
        else
        {
            messageUI.SetActive(true);
            Debug.Log("messageUI ON");
        }

        // Définit le texte de l’énigme
        if (messageText != null)
        {
            messageText.text =
                "Les anges ne peuvent être vaincus qu’en résolvant l’énigme.\nIndice : ordre → BDFH";

            Debug.Log("messageText updated");
        }
        else
        {
            Debug.LogWarning("messageText is NULL");
        }

        // Cache le message après quelques secondes
        StartCoroutine(HideMessage());

        // Vérifie le spawner
        if (spawner == null)
        {
            Debug.LogError("spawner is NULL");
            return;
        }

        // Spawn de l’ange
        Debug.Log("Spawning Angel...");
        GameObject angel = spawner.SpawnAngel();

        if (angel == null)
        {
            Debug.LogError("Angel spawn FAILED");
            return;
        }

        Debug.Log("Angel spawned: " + angel.name);

        // Vérifie script Angel
        Angel a = angel.GetComponent<Angel>();

        if (a == null)
            Debug.LogError("Angel script missing on prefab");
        else
            Debug.Log("Angel script OK");

        // Associe l’ange au GameManager
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager instance NULL");
            return;
        }

        GameManager2D.instance.angle = angel;
        Debug.Log("Angel assigned to GameManager");

        // Active les objets du puzzle (les chiffres)
        if (numbers == null)
        {
            Debug.LogError("numbers is NULL");
            return;
        }

        Debug.Log("Activating numbers...");

        for (int i = 0; i < numbers.Length; i++)
        {
            if (numbers[i] == null)
            {
                Debug.LogWarning("Number index " + i + " is NULL");
                continue;
            }

            numbers[i].SetActive(true);
            Debug.Log("Activated number: " + numbers[i].name);
        }
    }

    // Cache le message après 3 secondes
    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(3f);

        if (messageUI != null)
        {
            messageUI.SetActive(false);
            Debug.Log("UI hidden");
        }
    }

    // Empêche l’ange de sortir de la zone circulaire
    public Vector2 ClampPosition(Vector2 pos)
    {
        Vector2 dir = pos - (Vector2)center.position;

        if (dir.magnitude > radius)
            dir = dir.normalized * radius;

        return (Vector2)center.position + dir;
    }
}