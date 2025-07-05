using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTimerSeconds;
    [SerializeField] private TextMeshProUGUI textTimerMinutes;
    private static int maxTime;
    [SerializeField] private int MaxTime;
    private static int currentTime;

    public void StartTimer()
    {
        maxTime = MaxTime;
        currentTime = maxTime;
        UpdateTimerText();
        if (UIManager.onTimerStartDelegate != null)
        {
            UIManager.onTimerStartDelegate();
        }
        if (UIManager.onTimerStartDelegateLate != null)
        {
            UIManager.onTimerStartDelegateLate();
        }

        StartCoroutine("Countdown");
    }

    private void EndTimer()
    {
        if (UIManager.onTimerEndDelegate != null)
        {
            UIManager.onTimerEndDelegate();
        }
    }

    private IEnumerator Countdown()
    {
        while(currentTime > 0)
        {
            yield return new WaitForSeconds(1);
            currentTime--;
            UpdateTimerText();
        }
        if (UIManager.onTimerEndDelegate != null)
        {
            UIManager.onTimerEndDelegate();
        }
        EndTimer();
    }

    private void UpdateTimerText()
    {
        textTimerMinutes.text = $"{(currentTime / 60):D2}";
        textTimerSeconds.text = $"{(currentTime % 60):D2}";
    }
}
