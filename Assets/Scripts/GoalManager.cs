using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private GoalItemInteractable[] goalItemInteractables;
    [SerializeField] private GoalPillar[] goalPillars;
    [SerializeField] private MonsterAI monster;
    public static int maxScore;
    public static int currentScore;

    private void Awake()
    {
        StartCoroutine("AwakeCoroutine");
    }

    private IEnumerator AwakeCoroutine()
    {
        foreach(GoalPillar goalPillar in goalPillars)
        {
            maxScore++;
            goalPillar.onAcceptItem += () =>
            {
                Debug.Log("BASED ITEM!");
                goalPillar.SetIsUsed(true);
                monster.OnAcceptItem();
                currentScore++;
                if(currentScore >= maxScore)
                {
                    //What happens when player places all goal items onto the pillars
                }
            };
            goalPillar.onRejectItem += () =>
            {
                Debug.Log("CRINGE ITEM!");
            };
            goalPillar.onRemoveItem += () =>
            {
                currentScore--;
            };
        }
        yield return null;
    }
}
