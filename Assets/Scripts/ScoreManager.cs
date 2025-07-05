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
        animator.SetInteger("animatorScore", score);
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

    private void ResetHoop()
    {
        animator.SetBool("timerEnd", true);
    }

    private void StartHoop()
    {
        if(score != 0)
        {
            ResetScore();
        }
        animator.SetBool("timerEnd", false);
    }

    private void Start()
    {
        gainedScore = GainedScore;
        lostScore = LostScore;

        DisableScoring();

        UIManager.onTimerStartDelegate += ResetScore;
        UIManager.onTimerStartDelegate += EnableScoring;

        UIManager.onTimerStartDelegateLate += StartHoop;

        UIManager.onTimerEndDelegate += DisableScoring;
        UIManager.onTimerEndDelegate += ResetHoop;

    }

    public void GainScore()
    {
        if(canScore)
        {
            score += gainedScore;
            UpdateScore();
            if(onScoreDelegate!=null)
            {
                onScoreDelegate();
            }
            else
            {
                Debug.Log("Nothing in onScoreDelegate.");
            }
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
}
