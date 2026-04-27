using UnityEngine;
using TMPro;
//doha
public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeRemaining = 300f; // 5 minutes
    public bool timerIsRunning = true;

    void Update()
    {
        if (!timerIsRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            timeRemaining = 0;
            timerIsRunning = false;
            timerText.text = "TIME OVER";
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
