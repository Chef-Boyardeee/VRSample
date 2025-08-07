using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    public static Transform playerSpawnStatic;

    public static GameManager Instance;

    public delegate void OnStartGame();
    public static OnStartGame onStartGame;

    public delegate void OnRestartGame();
    public static OnRestartGame onRestartGame;

    public delegate void OnQuitGame();
    public static OnQuitGame onQuitGame;

    private void Awake()
    {
        StartCoroutine("AwakeCoroutine");
    }

    private IEnumerator AwakeCoroutine()
    {
        Instance = this;
        playerSpawnStatic = playerSpawn;
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
