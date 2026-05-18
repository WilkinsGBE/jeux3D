using UnityEngine;
using UnityEngine.UI;

public class Timer2D : MonoBehaviour
{
    public float time = 0f;
    public Text timerText;

    void Update()
    {
        time += Time.deltaTime;
        if (timerText != null)
            timerText.text = "Time: " + Mathf.FloorToInt(time);
    }
}
