using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager2 : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject victoryScreen;

    private void Awake()
    {
        GameManager.onStartGame += () =>
        {
            mainMenuScreen.SetActive(false);
            victoryScreen.SetActive(false);
        };

        GameManager.onRestartGame += () =>
        {
            mainMenuScreen.SetActive(false);
            victoryScreen.SetActive(false);
        };

        GoalManager.onVictory += () =>
        {
            mainMenuScreen.SetActive(false);
            victoryScreen.SetActive(true);
        };
    }
}
