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
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool canSeePlayer = distanceToPlayer < sightRange && HasLineOfSight();

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
        if (Physics.Raycast(transform.position + Vector3.up, (player.position - transform.position).normalized, out hit, sightRange))
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
        state = AIState.Roaming;
        yield return new WaitForSeconds(0.5f);

        Vector3 offset = Random.onUnitSphere * teleportDistance;
        offset.y = 0;
        Vector3 teleportPosition = player.position + offset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(teleportPosition, out hit, 5f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }

        ChooseNewRoamPoint();
    }

    void ChooseNewRoamPoint()
    {
        currentRoamTarget = roamPoints[Random.Range(0, roamPoints.Length)];
        agent.SetDestination(currentRoamTarget.position);
    }
}
