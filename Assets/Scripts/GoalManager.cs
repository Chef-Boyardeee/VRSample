using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private GoalItemInteractable[] goalItemInteractables;
    [SerializeField] private GoalPillar[] goalPillars;
    [SerializeField] private Transform[] goalItemSpawns;
    [SerializeField] private MonsterAI monster;
    public static int maxScore;
    public static int currentScore;

    private void Awake()
    {
        StartCoroutine("AwakeCoroutine");
    }

    private IEnumerator AwakeCoroutine()
    {
        //Check if there are enough goal item spawns
        if(goalItemInteractables.Length > goalItemSpawns.Length)
        {
            Debug.LogError("Not enough spawn locations.");
        }

        //Reset used goal item spawns
        Transform[] usedSpawns = new Transform[goalItemSpawns.Length];

        //Initialize goal item spawns
        foreach(GoalItemInteractable goalItem in goalItemInteractables)
        {
            bool goodSpawn = false;
            int i = 0;
            while (!goodSpawn)
            {
                i = Random.Range(0, goalItemSpawns.Length);
                if(usedSpawns[i] == goalItemSpawns[i])
                {
                    goodSpawn = false;
                }
                else
                {
                    goalItem.transform.position = goalItemSpawns[i].position;
                    usedSpawns[i] = goalItemSpawns[i];
                    goodSpawn = true;
                }
            }
        }

        //Initialize goal pillars
        foreach (GoalPillar goalPillar in goalPillars)
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
                    Debug.Log("Congratulations, you have escaped!");
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
