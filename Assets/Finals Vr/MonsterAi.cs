using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MonsterAI : MonoBehaviour
{
    // ===== Delegate for death event =====
    public static System.Action OnDeath;

    public Transform player;
    public float sightRange = 15f;
    public float loseSightTime = 5f;
    public float teleportDistance = 10f;
    public float speedBoostAmount = 2f;
    public float maxSpeed = 10f;
    public Transform[] roamPoints;

    private NavMeshAgent agent;
    private AIState state = AIState.Roaming;
    private float timeSinceLastSeen;
    private Transform currentRoamTarget;
    private bool isTeleporting = false;
    private bool wasSeeingPlayerLastFrame = false;
    private AIState lastLoggedState;
    private bool isActive = false; // Controls if monster can act

    private enum AIState { Roaming, Chasing, Searching, Teleporting }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        gameObject.SetActive(false); // Start hidden until first item given
        lastLoggedState = state;
        Debug.Log($"[MonsterAI] Initial State: {state}");
    }

    void Update()
    {
        if (!isActive) return; // Don't run AI if not active

        if (player == null)
        {
            Debug.LogWarning("MonsterAI: Player not assigned!");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool canSeePlayer = distanceToPlayer < sightRange && HasLineOfSight();

        // Logging sight change
        if (canSeePlayer && !wasSeeingPlayerLastFrame)
        {
            Debug.Log("I see you");
        }
        else if (!canSeePlayer && wasSeeingPlayerLastFrame)
        {
            Debug.Log("I don't see you");
        }
        wasSeeingPlayerLastFrame = canSeePlayer;

        // State change logging
        if (state != lastLoggedState)
        {
            Debug.Log($"[MonsterAI] State changed: {lastLoggedState} ? {state}");
            lastLoggedState = state;
        }

        // State behavior
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
        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = (player.position - origin).normalized;
        float distance = Vector3.Distance(origin, player.position);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
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
        if (canSeePlayer)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            timeSinceLastSeen = 0f;
            state = AIState.Searching;
            agent.ResetPath();
        }
    }

    void HandleSearching(bool canSeePlayer)
    {
        if (canSeePlayer)
        {
            state = AIState.Chasing;
            return;
        }

        timeSinceLastSeen += Time.deltaTime;

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

        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 5f, NavMesh.AllAreas))
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

    public void OnAcceptItem()
    {
        if (!isActive)
        {
            // First time activation
            gameObject.SetActive(true);
            isActive = true;
            Debug.Log("[MonsterAI] Activated after first item.");
        }

        agent.gameObject.SetActive(true);
        agent.speed = Mathf.Min(agent.speed + speedBoostAmount, maxSpeed);
        Debug.Log("GG");
    }

    // Detect player contact
    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.transform == player)
        {
            Debug.Log("[MonsterAI] Player caught!");
            GameManager.onDeath?.Invoke();
        }
    }
}
