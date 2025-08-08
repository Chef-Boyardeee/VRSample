using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    public static Transform playerSpawnStatic;

    [SerializeField] private GameObject player;
    public static GameObject playerStatic;

    [SerializeField] private TeleportationProvider provider;

    [SerializeField] private Material night;
    [SerializeField] private Material day;

    public static GameManager Instance;

    public delegate void OnStartGame();
    public static OnStartGame onStartGame;

    public delegate void OnRestartGame();
    public static OnRestartGame onRestartGame;

    public delegate void OnQuitGame();
    public static OnQuitGame onQuitGame;

    public delegate void OnDeath();
    public static OnDeath onDeath;

    private void Awake()
    {
        StartCoroutine("AwakeCoroutine");
    }

    private IEnumerator AwakeCoroutine()
    {
        Instance = this;
        playerSpawnStatic = playerSpawn;
        playerStatic = player;

        onStartGame += () =>
        {
            TeleportRequest request = new TeleportRequest()
            {
                destinationPosition = playerSpawn.position,
                destinationRotation = playerSpawn.rotation,
                matchOrientation = MatchOrientation.WorldSpaceUp
            };
            provider.QueueTeleportRequest(request);
            player.transform.position = playerSpawn.position;
            RenderSettings.skybox = night;
        };

        onRestartGame += () =>
        {
            TeleportRequest request = new TeleportRequest()
            {
                destinationPosition = playerSpawn.position,
                destinationRotation = playerSpawn.rotation,
                matchOrientation = MatchOrientation.WorldSpaceUp
            };
            provider.QueueTeleportRequest(request);

            player.transform.position = playerSpawn.position;
            RenderSettings.skybox = night;
        };

        onDeath += () =>
        {
            RenderSettings.skybox = night;
        };

        GoalManager.onVictory += () =>
        {
            TeleportRequest request = new TeleportRequest()
            {
                destinationPosition = playerSpawn.position,
                destinationRotation = playerSpawn.rotation,
                matchOrientation = MatchOrientation.WorldSpaceUp
            };
            provider.QueueTeleportRequest(request);

            player.transform.position = playerSpawn.position;
            RenderSettings.skybox = day;
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
