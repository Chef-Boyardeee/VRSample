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

    public delegate void OnVictory();
    public static OnVictory onVictory;

    private void Awake()
    {
        GameManager.onStartGame += InitializeGoalItems;
        GameManager.onStartGame += InitializeGoalPillars;
        //Add enemy reset to onStartGame

        GameManager.onRestartGame += InitializeGoalItems;
        GameManager.onRestartGame += InitializeGoalPillars;
        //Add enemy reset to onRestartGame
    }

    private void InitializeGoalItems()
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
    }

    public void InitializeGoalPillars()
    {
        currentScore = 0;
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
}
