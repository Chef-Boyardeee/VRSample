using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private void Awake()
    {
        StartCoroutine("AwakeCoroutine");
    }

    private IEnumerator AwakeCoroutine()
    {
        Instance = this;
        GameManager.onStartGame += () =>
        {
            gameObject.transform.position = GameManager.playerSpawnStatic.position;
        };
        GameManager.onRestartGame += () =>
        {
            gameObject.transform.position = GameManager.playerSpawnStatic.position;
        };
        yield return null;
    }
}
