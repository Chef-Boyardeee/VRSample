using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static bool canScore;
    private static int score;
    private static int gainedScore;
    [SerializeField] private int GainedScore;
    private static int lostScore;
    [SerializeField] private int LostScore;


    [SerializeField] private TextMeshProUGUI textScore;

    private static ScoreManager instance;
    public static ScoreManager Instance => instance;

    private void Awake()
    {
        StartCoroutine("AwakeCoroutine");
    }

    private IEnumerator AwakeCoroutine()
    {
        instance = this;
        yield return null;
    }

    private void UpdateScore()
    {
        textScore.text = $"Score: {score}";
    }

    private void EnableScoring()
    {
        canScore = true;
    }

    private void DisableScoring()
    {
        canScore = false;
    }

    private void ResetScore()
    {
        score = 0;
        UpdateScore();
    }

    private void Start()
    {
        gainedScore = GainedScore;
        lostScore = LostScore;

        DisableScoring();

        UIManager.onTimerStartDelegate += EnableScoring;
        UIManager.onTimerStartDelegate += ResetScore;

        UIManager.onTimerEndDelegate += DisableScoring;
    }

    public void GainScore()
    {
        if(canScore)
        {
            score += gainedScore;
            UpdateScore();
        }
    }

    public void LoseScore()
    {
        if (canScore && score >= lostScore)
        {
            score -= lostScore;
            UpdateScore();
        }
    }
}
