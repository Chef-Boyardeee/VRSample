using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager2 : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject deathScreen;

    private void Awake()
    {
        GameManager.onStartGame += () =>
        {
            mainMenuScreen.SetActive(false);
            victoryScreen.SetActive(false);
            deathScreen.SetActive(false);
        };

        GameManager.onRestartGame += () =>
        {
            mainMenuScreen.SetActive(false);
            victoryScreen.SetActive(false);
            deathScreen.SetActive(false);
        };

        GoalManager.onVictory += () =>
        {
            mainMenuScreen.SetActive(false);
            victoryScreen.SetActive(true);
            deathScreen.SetActive(false);
        };

        GameManager.onDeath() += () =>
        {
            mainMenuScreen.SetActive(false);
            victoryScreen.SetActive(false);
            deathScreen.SetActive(true);
        };
    }
}
