using UnityEngine;
using TMPro;

// Script de timer (compte à rebours du jeu)
public class Timer : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI timerText; // texte affiché à l'écran

    [Header("Time Settings")]
    public float timeRemaining = 600f; // 10 minutes en secondes
    public bool timerIsRunning = true; // contrôle si le timer tourne ou non

    void Update()
    {
        // si le timer est arrêté → on ne fait rien
        if (!timerIsRunning) return;

        // tant qu'il reste du temps
        if (timeRemaining > 0)
        {
            // décrémente le temps
            timeRemaining -= Time.deltaTime;

            // met à jour l'affichage
            DisplayTime(timeRemaining);
        }
        else
        {
            // fin du timer
            timeRemaining = 0;
            timerIsRunning = false;

            // affichage fin de temps
            timerText.text = "TIME OVER";
        }
    }

    // convertit les secondes en minutes:secondes
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // affichage formaté 00:00
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}