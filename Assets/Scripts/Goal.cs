using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //private static bool hasScored;

    private void Start()
    {
        //hasScored = false;
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnCollisionExit(Collision collision)
    {
        //hasScored = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        ScoreManager.Instance.GainScore();
    }
}
