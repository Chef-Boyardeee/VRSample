using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTimer;
    private static int maxTime;
    [SerializeField] private int MaxTime;
    private static int currentTime;

    public void StartTimer()
    {
        Debug.Log("Timer started.");
        maxTime = MaxTime;
        currentTime = maxTime;
        UpdateTimerText();
        if (UIManager.onTimerStartDelegate != null)
        {
            UIManager.onTimerStartDelegate();
        }
        StartCoroutine("Countdown");
    }

    private void EndTimer()
    {
        Debug.Log("Timer ended.");
        if (UIManager.onTimerEndDelegate != null)
        {
            UIManager.onTimerEndDelegate();
        }
    }

    private IEnumerator Countdown()
    {
        Debug.Log("Timer counting down.");
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
        textTimer.text = currentTime.ToString();
    }
}
