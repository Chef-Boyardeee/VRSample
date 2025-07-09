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

    private enum AIState { Roaming, Chasing, Searching, Teleporting }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChooseNewRoamPoint();
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

        Debug.Log($"[MonsterAI] State: {state}, CanSeePlayer: {canSeePlayer}, Distance: {distanceToPlayer}");

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
                StartCoroutine(TeleportNearPlayer());
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
            Debug.Log("Switching to Chasing");
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
            Debug.Log("Lost sight of player. Searching...");
            timeSinceLastSeen = 0f;
            state = AIState.Searching;
        }
    }

    void HandleSearching(bool canSeePlayer)
    {
        timeSinceLastSeen += Time.deltaTime;

        if (canSeePlayer)
        {
            Debug.Log("Player found again. Chasing!");
            state = AIState.Chasing;
            return;
        }

        if (timeSinceLastSeen > loseSightTime)
        {
            Debug.Log("Can't find player. Teleporting...");
            state = AIState.Teleporting;
        }
    }

    IEnumerator TeleportNearPlayer()
    {
        yield return new WaitForSeconds(0.5f); // Optional delay

        Vector3 offset = Random.onUnitSphere * teleportDistance;
        offset.y = 0;
        Vector3 targetPos = player.position + offset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 5f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
            Debug.Log("Teleported near player");
        }

        state = AIState.Roaming;
        ChooseNewRoamPoint();
    }

    void ChooseNewRoamPoint()
    {
        if (roamPoints.Length == 0) return;
        currentRoamTarget = roamPoints[Random.Range(0, roamPoints.Length)];
        agent.SetDestination(currentRoamTarget.position);
    }
}
