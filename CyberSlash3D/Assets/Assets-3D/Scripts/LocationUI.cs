using UnityEngine;
using TMPro;
using System.Collections;

public class LocationUI : MonoBehaviour
{
    public static LocationUI instance;

    public CanvasGroup panelGroup;
    public TMP_Text locationText;

    public float displayTime = 5f;
    public float fadeSpeed = 2f;

    private Coroutine currentRoutine;

    void Awake()
    {
        instance = this;

        if (panelGroup != null)
        {
            panelGroup.alpha = 0f;
            panelGroup.gameObject.SetActive(false);
        }
    }

    public void ShowLocation(string locationName)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(locationName));
    }

    private IEnumerator ShowRoutine(string name)
    {
        panelGroup.gameObject.SetActive(true);
        locationText.text = name;

        yield return Fade(1f);

        yield return new WaitForSeconds(displayTime);

        yield return Fade(0f);

        panelGroup.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        while (!Mathf.Approximately(panelGroup.alpha, targetAlpha))
        {
            panelGroup.alpha = Mathf.MoveTowards(
                panelGroup.alpha,
                targetAlpha,
                fadeSpeed * Time.deltaTime
            );

            yield return null;
        }
    }
}