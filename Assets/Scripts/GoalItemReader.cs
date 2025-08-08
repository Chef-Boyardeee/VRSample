using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalItemReader : MonoBehaviour
{
    private GoalItemInteractable item;

    private void OnTriggerEnter(Collider other)
    {
        item = other.gameObject.GetComponent<GoalItemInteractable>();
    }

    private void OnTriggerStay(Collider other)
    {
        item = other.gameObject.GetComponent<GoalItemInteractable>();
    }

    private void OnTriggerExit(Collider other)
    {
        item = null;
    }

    public GoalItemInteractable GetItem()
    {
        return item;
    }
}
