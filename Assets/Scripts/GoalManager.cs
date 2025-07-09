using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private GoalItemInteractable[] goalItemInteractables;
    [SerializeField] private GoalPillar[] goalPillars;

    private void Awake()
    {
        StartCoroutine("AwakeCoroutine");
    }

    private IEnumerator AwakeCoroutine()
    {
        foreach(GoalPillar goalPillar in goalPillars)
        {
            foreach(GoalItemInteractable goalItemInteractable in goalItemInteractables)
            {
                goalPillar.onAcceptItem += goalItemInteractable.OnAcceptItem;
                if (goalItemInteractable.GetItem() == goalPillar.GetItem())
                {
                    goalPillar.onAcceptItem += goalItemInteractable.OnAcceptItem;
                    break;
                }
            }
        }
        yield return null;
    }
}
