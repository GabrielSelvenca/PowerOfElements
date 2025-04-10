using System.Collections;
using TMPro;
using UnityEngine;

public class CrystalsTimerController : MonoBehaviour
{
    public TextMeshProUGUI SpeedText;
    public TextMeshProUGUI JumpText;
    public TextMeshProUGUI HealthText;

    private Coroutine speedTextRoutine;
    private Coroutine jumpTextRoutine;
    private Coroutine healthTextRoutine;
    
    public void SpeedTimer(float duration)
    {
        if (speedTextRoutine != null)
            StopCoroutine(speedTextRoutine);

        speedTextRoutine = StartCoroutine(UpdateUIRoutine(SpeedText, duration));
    }

    public void JumpTimer(float duration)
    {
        if (jumpTextRoutine != null)
            StopCoroutine(jumpTextRoutine);

        jumpTextRoutine = StartCoroutine(UpdateUIRoutine(JumpText, duration));
    }

    public void HealthTimer(float duration)
    {
        if (healthTextRoutine != null)
            StopCoroutine(healthTextRoutine);

        healthTextRoutine = StartCoroutine(UpdateUIRoutine(HealthText, duration));
    }

    IEnumerator UpdateUIRoutine(TextMeshProUGUI text, float duration)
    {
        float timeLeft = duration;

        while (timeLeft > 0)
        {
            int totalSeconds = Mathf.CeilToInt(timeLeft);
            int minutos = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            
            if (minutos <= 0 && seconds <= 10)
            {
                text.color = Color.red;
            } else if (minutos <= 1)
            {
                text.color = Color.yellow;
            } else
            {
                text.color = Color.green;
            }

            text.text = $"{minutos:00}:{seconds:00}";

            timeLeft -= Time.deltaTime;
            yield return null;
        }

        text.text = "00:00";
        text.color = Color.white;
    }
}
