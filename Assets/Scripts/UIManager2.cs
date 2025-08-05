using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager2 : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuScreen;

    private void Awake()
    {
        GameManager.onStartGame += () =>
        {
            mainMenuScreen.SetActive(false);
        };
    }
}
