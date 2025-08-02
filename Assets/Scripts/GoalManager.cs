using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private GoalItemInteractable[] goalItemInteractables;
    [SerializeField] private GoalPillar[] goalPillars;
    [SerializeField] private MonsterAI monster;

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
                    goalPillar.onAcceptItem += monster.OnAcceptItem;
                    goalPillar.onAcceptItem += () =>
                    {
                        Debug.Log("BASED ITEM!");
                        goalPillar.SetIsUsed(true);
                    };
                }
                else
                {
                    goalPillar.onRejectItem += () =>
                    {
                        Debug.Log("CRINGE ITEM!");
                    };
                }
            }
        }
        yield return null;
    }
}
