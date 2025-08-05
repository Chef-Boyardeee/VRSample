using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public delegate void OnStartGame();
    public static OnStartGame onStartGame;

    public delegate void OnRestartGame();
    public static OnRestartGame onRestartGame;

    public delegate void OnQuitGame();
    public static OnQuitGame onQuitGame;

    void Awake()
    {
        Instance = this;
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
