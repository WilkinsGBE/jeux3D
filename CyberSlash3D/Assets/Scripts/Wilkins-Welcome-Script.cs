using UnityEngine;
using System.Collections;

public class WelcomeZone : MonoBehaviour
{
    public GameObject welcomePanel;
    public float displayTime = 4f;
    public float fadeSpeed = 2f;
    public Vector3 gizmoSize = new Vector3(3f, 2f, 3f);

    private bool triggered = false;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (welcomePanel != null)
        {
            canvasGroup = welcomePanel.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
                canvasGroup = welcomePanel.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            welcomePanel.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(ShowMessage());
        }
    }

    IEnumerator ShowMessage()
    {
        welcomePanel.SetActive(true);

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += fadeSpeed * Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(displayTime);

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;

        welcomePanel.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.3f);
        Gizmos.DrawCube(transform.position, gizmoSize);
        Gizmos.color = new Color(0f, 1f, 0.5f, 1f);
        Gizmos.DrawWireCube(transform.position, gizmoSize);
    }
}
