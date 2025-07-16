using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MonsterAI : MonoBehaviour
{
    public Transform player;
    public float sightRange = 15f;
    public float loseSightTime = 5f;
    public float teleportDistance = 10f;
    public Transform[] roamPoints;

    private NavMeshAgent agent;
    private AIState state = AIState.Roaming;
    private float timeSinceLastSeen;
    private Transform currentRoamTarget;
    private bool isTeleporting = false;
    private AIState lastLoggedState;

    private enum AIState { Roaming, Chasing, Searching, Teleporting }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChooseNewRoamPoint();
        lastLoggedState = state;
        Debug.Log($"[MonsterAI] Initial State: {state}");
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("MonsterAI: Player not assigned!");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool canSeePlayer = distanceToPlayer < sightRange && HasLineOfSight();

        if (state != lastLoggedState)
        {
            Debug.Log($"[MonsterAI] State changed: {lastLoggedState} ? {state}");
            lastLoggedState = state;
        }

        switch (state)
        {
            case AIState.Roaming:
                HandleRoaming(canSeePlayer);
                break;
            case AIState.Chasing:
                HandleChasing(canSeePlayer);
                break;
            case AIState.Searching:
                HandleSearching(canSeePlayer);
                break;
            case AIState.Teleporting:
                if (!isTeleporting)
                {
                    StartCoroutine(TeleportNearPlayer());
                }
                break;
        }
    }

    bool HasLineOfSight()
    {
        RaycastHit hit;
        Vector3 direction = (player.position - transform.position).normalized;
        if (Physics.Raycast(transform.position + Vector3.up, direction, out hit, sightRange))
        {
            return hit.transform == player;
        }
        return false;
    }

    void HandleRoaming(bool canSeePlayer)
    {
        if (canSeePlayer)
        {
            state = AIState.Chasing;
            return;
        }

        if (agent.remainingDistance < 1f)
        {
            ChooseNewRoamPoint();
        }
    }

    void HandleChasing(bool canSeePlayer)
    {
        agent.SetDestination(player.position);

        if (!canSeePlayer)
        {
            timeSinceLastSeen = 0f;
            state = AIState.Searching;
        }
    }

    void HandleSearching(bool canSeePlayer)
    {
        timeSinceLastSeen += Time.deltaTime;

        if (canSeePlayer)
        {
            state = AIState.Chasing;
            return;
        }

        if (timeSinceLastSeen > loseSightTime)
        {
            state = AIState.Teleporting;
        }
    }

    IEnumerator TeleportNearPlayer()
    {
        isTeleporting = true;

        yield return new WaitForSeconds(0.1f);

        agent.enabled = false;

        Vector3 offset = Random.onUnitSphere * teleportDistance;
        offset.y = 0;
        Vector3 targetPos = player.position + offset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 5f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            Debug.Log("[MonsterAI] Teleported near player.");
        }
        else
        {
            Debug.LogWarning("[MonsterAI] Failed to find valid teleport position.");
        }

        agent.enabled = true;

        state = AIState.Roaming;
        ChooseNewRoamPoint();

        isTeleporting = false;
    }

    void ChooseNewRoamPoint()
    {
        if (roamPoints.Length == 0) return;
        currentRoamTarget = roamPoints[Random.Range(0, roamPoints.Length)];
        agent.SetDestination(currentRoamTarget.position);
    }
}
