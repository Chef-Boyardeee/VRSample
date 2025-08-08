using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    public static Transform playerSpawnStatic;

    [SerializeField] private GameObject player;
    public static GameObject playerStatic;

    public static GameManager Instance;

    public delegate void OnStartGame();
    public static OnStartGame onStartGame;

    public delegate void OnRestartGame();
    public static OnRestartGame onRestartGame;

    public delegate void OnQuitGame();
    public static OnQuitGame onQuitGame;

    public delegate void OnDeath();
    public static OnDeath onDeath();

    private void Awake()
    {
        StartCoroutine("AwakeCoroutine");
    }

    private IEnumerator AwakeCoroutine()
    {
        Instance = this;
        playerSpawnStatic = playerSpawn;
        playerStatic = player;

        onRestartGame += () =>
        {
            playerStatic.transform.position = playerSpawnStatic.position;
        };

        GoalManager.onVictory += () =>
        {
            playerStatic.transform.position = playerSpawnStatic.position;
        };
        yield return null;
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER! Restarting scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame()
    {
        Debug.Log("Yo");
        onStartGame?.Invoke();
    }

    public void RestartGame()
    {
        Debug.Log("Fein");
        onRestartGame?.Invoke();
    }

    public void QuitGame()
    {
        Debug.Log("Bye");
        Application.Quit();
    }
}
