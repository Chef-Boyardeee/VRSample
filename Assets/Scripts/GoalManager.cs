using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private GoalItemInteractable[] goalItemInteractables;
    [SerializeField] private GoalPillar[] goalPillars;
    [SerializeField] private Transform[] goalItemSpawns;
    [SerializeField] private MonsterAI monster;
    [SerializeField] private Transform waitingArea;

    public static int maxScore;
    public static int currentScore;

    public delegate void OnVictory();
    public static OnVictory onVictory;

    private void Awake()
    {
        GameManager.onStartGame += ReinitializeGoalItems;
        GameManager.onStartGame += InitializeGoalItems;
        GameManager.onStartGame += InitializeGoalPillars;
        //Add enemy reset to onStartGame

        GameManager.onRestartGame += ReinitializeGoalItems;
        GameManager.onRestartGame += InitializeGoalItems;
        GameManager.onRestartGame += InitializeGoalPillars;
        //Add enemy reset to onRestartGame
    }

    private void InitializeGoalItems()
    {
        //Check if there are enough goal item spawns
        if(goalItemInteractables.Length > goalItemSpawns.Length || goalPillars.Length > goalItemInteractables.Length)
        {
            Debug.LogError("Goal-related error.");
        }

        //Reset used goal item spawns
        Transform[] usedGoalItemSpawns = new Transform[goalItemSpawns.Length];

        //Initialize goal item spawns
        foreach(GoalItemInteractable goalItem in goalItemInteractables)
        {
            goalItem.gameObject.SetActive(true);
            bool goodSpawn = false;
            int i;
            if(goalItem.GetItem().isFirstItem)
            {
                goalItem.transform.position = goalItemSpawns[0].position;
                usedGoalItemSpawns[0] = goalItemSpawns[0];
                goodSpawn = true;
            }
            while (!goodSpawn)
            {
                i = Random.Range(0, goalItemSpawns.Length);
                if(usedGoalItemSpawns[i] == goalItemSpawns[i])
                {
                    goodSpawn = false;
                }
                else
                {
                    goalItem.transform.position = goalItemSpawns[i].position;
                    usedGoalItemSpawns[i] = goalItemSpawns[i];
                    goodSpawn = true;
                }
            }
        }
    }

    public void InitializeGoalPillars()
    {
        currentScore = 0;
        maxScore = 0;

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
                if (currentScore >= maxScore)
                {
                    //What happens when player places all goal items onto the pillars
                    Debug.Log("Congratulations, you have escaped!");
                    onVictory?.Invoke();
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
    }

    public void ReinitializeGoalItems()
    {
        /*foreach(GoalItemInteractable goalItem in goalItemInteractables)
        {
            foreach (GoalPillar goalPillar in goalPillars)
            {
                goalPillar.interactionManager.SelectExit(goalPillar, itemSocketed)
            }
        }*/
        Debug.Log("Reinitialize called.");
        foreach (GoalPillar goalPillar in goalPillars)
        {
            goalPillar.gameObject.SetActive(false);
        }

        foreach(GoalItemInteractable goalItem in goalItemInteractables)
        {
            goalItem.gameObject.SetActive(false);
            goalItem.transform.position = waitingArea.position;
        }

        foreach (GoalPillar goalPillar in goalPillars)
        {
            goalPillar.gameObject.SetActive(true);
        }

        /*foreach (GoalPillar goalPillar in goalPillars)
        {
            if(goalPillar.hasSelection)
            {
                GoalItemInteractable item = goalPillar.GetReader().GetItem();
                goalPillar.interactionManager.SelectExit(goalPillar, item);
                item.transform.position = waitingArea.position;
            }
        }*/
    }

    public void DeactivateGoalItems()
    {
        foreach (GoalItemInteractable goalItem in goalItemInteractables)
        {
            goalItem.gameObject.SetActive(false);
        }
    }
}
