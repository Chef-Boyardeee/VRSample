using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private static bool canScore;
    private static int score;
    private static int gainedScore;
    [SerializeField] private int GainedScore;
    private static int lostScore;
    [SerializeField] private int LostScore;

    [SerializeField] private TextMeshProUGUI textScore;

    private static ScoreManager instance;
    public static ScoreManager Instance => instance;

    public delegate void OnScore();
    public static OnScore onScoreDelegate;

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
        textScore.text = score.ToString();
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

        onScoreDelegate += SlowHoop;
        onScoreDelegate += FastHoop;

        UIManager.onTimerStartDelegate += EnableScoring;
        UIManager.onTimerStartDelegate += ResetScore;
        UIManager.onTimerStartDelegate += StartHoop;

        UIManager.onTimerEndDelegate += DisableScoring;
        UIManager.onTimerEndDelegate += ResetHoop;

    }

    public void GainScore()
    {
        if(canScore)
        {
            onScoreDelegate();
            score += gainedScore;
            UpdateScore();
        }
    }

    private void LoseScore()
    {
        if (canScore && score >= lostScore)
        {
            score -= lostScore;
            UpdateScore();
        }
    }

    private void SlowHoop()
    {
        if(score >= 20)
        {
            animator.SetBool("scoredTwenty", true);
        }
    }

    private void FastHoop()
    {
        if (score >= 50)
        {
            animator.SetBool("scoredFifty", true);
        }
    }

    private void ResetHoop()
    {
        animator.SetBool("timerEnd", true);
        animator.SetBool("scoredFifty", false);
        animator.SetBool("scoredTwenty", false);
    }

    private void StartHoop()
    {
        animator.SetBool("timerEnd", false);
    }
}
