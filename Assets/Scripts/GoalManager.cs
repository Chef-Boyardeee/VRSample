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
                if (goalItemInteractable.GetItem() == goalPillar.GetItem())
                {
                    goalPillar.onAcceptItem += goalItemInteractable.OnAcceptItem;
                }
                else
                {
                    goalPillar.onRejectItem += goalItemInteractable.OnRejectItem;
                }
            }
        }
        yield return null;
    }
}
