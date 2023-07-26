using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent enemy;

    public LayerMask groundLayer, playerLayer;

    // Patrol
    private bool isPatrolling;
    public float walkRange;

    // State
    public float sightRange;
    public float fovAngle = 90f; // Field of view angle
    private bool playerInSight;
    private enum EnemyState { Patrol, Chase, Idle };
    private EnemyState currentState;

    private Vector3 originalPosition; // Store the original position for patrolling back
    private Vector3 lastKnownPlayerPosition; // Store the last known player position

    void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform; // Assuming the player has the "Player" tag.
        currentState = EnemyState.Patrol;
        originalPosition = transform.position;
        lastKnownPlayerPosition = originalPosition;
    }

    void Update()
    {
        playerInSight = PlayerInSight();

        switch (currentState)
        {
            case EnemyState.Patrol:
                if (!playerInSight)
                {
                    Patrol();
                }
                else
                {
                    lastKnownPlayerPosition = player.position; // Store the player's position when detected
                    currentState = EnemyState.Chase;
                }
                break;

            case EnemyState.Chase:
                if (playerInSight)
                {
                    lastKnownPlayerPosition = player.position; // Update the last known position while chasing
                    Chase();
                }
                else
                {
                    currentState = EnemyState.Idle;
                    StartCoroutine(IdleBeforePatrol());
                }
                break;

            case EnemyState.Idle:
                // Do nothing and wait for IdleBeforePatrol to finish
                break;
        }
    }

    bool PlayerInSight()
    {
        // Check if the player is within the sight range
        if (Vector3.Distance(transform.position, player.position) <= sightRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            // Calculate the angle between the enemy's forward direction and the direction to the player
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            // Check if the player is within the FOV angle
            if (angleToPlayer < fovAngle * 0.5f)
            {
                // Perform a raycast directly towards the player using the playerLayer to detect the player
                if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, sightRange, playerLayer))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    void Patrol()
    {
        if (!isPatrolling)
        {
            StartCoroutine(SearchDestination());
        }

        if (isPatrolling)
        {
            // Check if the enemy has reached the destination
            if (!enemy.pathPending && enemy.remainingDistance < 0.1f)
            {
                // Resume patrolling by starting a new search for the destination
                StartCoroutine(SearchDestination());
            }
        }
    }

    IEnumerator SearchDestination()
    {
        isPatrolling = true;
        Vector3 finalPosition = Vector3.zero;
        float minimumDistance = 10f; // Minimum distance for the patrol destination

        // Generate a random position within the walk range, ensuring it meets the minimum distance requirement
        while (true)
        {
            Vector3 randomDirection = Random.insideUnitSphere * walkRange + (transform.position - originalPosition).normalized * minimumDistance;

            // Ensure the position is on the NavMesh and reachable
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, walkRange, 1))
            {
                finalPosition = hit.position;
                break;
            }

            yield return null;
        }

        // Ensure the final position is on the NavMesh and reachable
        NavMeshHit finalHit;
        if (NavMesh.Raycast(transform.position, finalPosition - transform.position, out finalHit, NavMesh.AllAreas))
        {
            finalPosition = finalHit.position;
        }

        enemy.SetDestination(finalPosition);

        // Wait until the enemy reaches the destination
        while (Vector3.Distance(transform.position, finalPosition) > 1f)
        {
            yield return null;
        }

        isPatrolling = false;
    }

    void Chase()
    {
        enemy.SetDestination(player.position);
    }

    IEnumerator IdleBeforePatrol()
    {
        // Wait for a few seconds before going back to patrol
        yield return new WaitForSeconds(1f); // You can adjust the idle time here

        enemy.SetDestination(lastKnownPlayerPosition); // Go to the last known player position
        currentState = EnemyState.Chase; // Set to Chase state to continue chasing if the player comes back

        while (Vector3.Distance(transform.position, lastKnownPlayerPosition) > 1f)
        {
            yield return null;
        }

        currentState = EnemyState.Patrol; // Change back to Patrol state after reaching the last known position
    }

    // Draw Gizmos for FOV angle and sight radius
    void OnDrawGizmosSelected()
    {
        // Draw FOV angle Gizmos (yellow lines)
        Gizmos.color = Color.yellow;
        Vector3 fovLine1 = Quaternion.AngleAxis(fovAngle * 0.5f, transform.up) * transform.forward * sightRange;
        Vector3 fovLine2 = Quaternion.AngleAxis(-fovAngle * 0.5f, transform.up) * transform.forward * sightRange;

        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        // Draw sight radius Gizmo (red sphere)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // Draw the destination gizmo (blue sphere)
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(enemy.destination, 0.3f);

        // Draw a straight line from the enemy's current position to the destination
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, enemy.destination);
    }
}
