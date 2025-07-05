using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject panelTimer;
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelReplayMenu;
    [SerializeField] private GameObject panelScoreTracker;
    [SerializeField] private Timer timer;

    public delegate void OnTimerStart();
    public static OnTimerStart onTimerStartDelegate;

    public static OnTimerStart onTimerStartDelegateLate;


    public delegate void OnTimerEnd();
    public static OnTimerStart onTimerEndDelegate;

    private static UIManager instance;
    public static UIManager Instance => instance;

    private void Awake()
    {
        StartCoroutine("AwakeCoroutine");
    }

    private IEnumerator AwakeCoroutine()
    {
        instance = this;
        yield return null;
    }

    private void ShowUIStart()
    {
        panelTimer.gameObject.SetActive(false);
        panelReplayMenu.gameObject.SetActive(false);
        panelScoreTracker.gameObject.SetActive(false);
        panelMenu.gameObject.SetActive(true);
    }

    private void ShowUIGameStart()
    {
        panelTimer.gameObject.SetActive(true);
        panelReplayMenu.gameObject.SetActive(false);
        panelScoreTracker.gameObject.SetActive(true);
        panelMenu.gameObject.SetActive(false);
    }

    private void ShowUIGameEnd()
    {
        panelTimer.gameObject.SetActive(false);
        panelReplayMenu.gameObject.SetActive(true);
        panelScoreTracker.gameObject.SetActive(true);
        panelMenu.gameObject.SetActive(false);
    }

    private void Start()
    {
        ShowUIStart();

        onTimerStartDelegate += ShowUIGameStart;
        onTimerEndDelegate += ShowUIGameEnd;
    }

    public void StartGame()
    {
        timer.StartTimer();
    }

    public void EndGame()
    { 
        Application.Quit();
    }

}
