using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class RoamingState : IMonsterState
{
    private NavMeshAgent agent;
    private Transform monsterTransform;
    private float roamRadius;
    private float waitTimeBetweenRoam;

    private float timeSinceLastRoam;
    
    private Transform playerTransform; // Reference to the player's transform
    private LayerMask lineOfSightMask; // Layer mask to determine what objects block line of sight
    private float viewDistance; // How far the monster can see
    private float viewAngle; // The field of view angle for line of sight

    // Constructor
    public RoamingState(NavMeshAgent agent, Transform monsterTransform, Transform playerTransform, float roamRadius, float waitTimeBetweenRoam, LayerMask lineOfSightMask, float viewDistance, float viewAngle)
    {
        this.agent = agent;
        this.monsterTransform = monsterTransform;
        this.playerTransform = playerTransform;
        this.roamRadius = roamRadius;
        this.waitTimeBetweenRoam = waitTimeBetweenRoam;
        this.lineOfSightMask = lineOfSightMask;
        this.viewDistance = viewDistance;
        this.viewAngle = viewAngle;

    }

    public void OnEnterState()
    {
        agent.speed = 1.15f; // Set the roaming speed
        timeSinceLastRoam = waitTimeBetweenRoam; // Initiate the first roam immediately
    }

    public void OnUpdateState()
    {
        // Check for line of sight to the player
        if (HasLineOfSightToPlayer())
        {
            Debug.Log("Enemy Spotted");
            // Transition to the chasing state
            monsterTransform.GetComponent<MonsterController>().StartChasing();
            return;
        }
        if (agent.isStopped || agent.remainingDistance < 0.5f)
        {
            timeSinceLastRoam += Time.deltaTime;

            if (timeSinceLastRoam >= waitTimeBetweenRoam)
            {
                Vector3 newDestination = RandomNavSphere(monsterTransform.position, roamRadius);
                agent.SetDestination(newDestination);
                timeSinceLastRoam = 0f;
            }
        }
        
        
    }

    public void OnExitState()
    {
        // Any cleanup when exiting the roaming state
    }

    private Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);

        return navHit.position;
    }
    
    private bool HasLineOfSightToPlayer()
    {
        Vector3 directionToPlayer = playerTransform.position - monsterTransform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Check if the player is within view distance and the angle is within the field of view
        if (distanceToPlayer <= viewDistance && Vector3.Angle(monsterTransform.forward, directionToPlayer) <= viewAngle / 2)
        {
            RaycastHit hit;
            // Perform a raycast towards the player
            if (Physics.Raycast(monsterTransform.position, directionToPlayer.normalized, out hit, distanceToPlayer, lineOfSightMask))
            {
                // If the raycast hits the player, return true for line of sight
                return hit.transform.CompareTag("Player");
            }
        }

        return false;
    }
}


public class ChasingState : IMonsterState
{
    private NavMeshAgent agent;
    private Transform monsterTransform;
    private Transform playerTransform;
    private float chaseSpeed;
    private float reactionTime;

    private bool isChasing;
    private float reactionTimer;
    private float catchRadius; // The radius within which the player is considered caught
    public float CatchRadius => catchRadius;  // Public getter for the catch radius
    private float timeToLosePlayer = 6f; // Time in seconds to switch back to RoamingState if player is lost
    private float timeSinceLastSeenPlayer;
    
    private float viewRange; // Max distance to see the player
    private float fieldOfView; // Angle for the field of view
    private LayerMask obstacleMask; // LayerMask to determine what objects block line of sight

    public ChasingState(NavMeshAgent agent, Transform monsterTransform, Transform playerTransform, float chaseSpeed, float reactionTime, float catchRadius, float viewRange, float fieldOfView, LayerMask obstacleMask)
    {
        this.agent = agent;
        this.monsterTransform = monsterTransform;
        this.playerTransform = playerTransform;
        this.chaseSpeed = chaseSpeed;
        this.reactionTime = reactionTime;
        this.catchRadius = catchRadius;
        
        this.viewRange = viewRange;
        this.fieldOfView = fieldOfView;
        this.obstacleMask = obstacleMask;
    }

    public void OnEnterState()
    {
        // Set the chasing speed
        agent.speed = chaseSpeed;
        // Initialize the reaction timer
        reactionTimer = 0f;
        isChasing = false;
        timeSinceLastSeenPlayer = 0f; 
        // Start the coroutine for the reaction delay (this can also be done using a simple timer)
        monsterTransform.GetComponent<MonoBehaviour>().StartCoroutine(ReactionDelay());
    }

    public void OnUpdateState()
    {
        if (isChasing)
        {
            // Continue chasing the player
            agent.SetDestination(playerTransform.position);
            if (CanSeePlayer())
            {
                timeSinceLastSeenPlayer = 0f; // Reset the timer as the player is seen
                agent.SetDestination(playerTransform.position);
                // Check if the player is within catch radius
                if (Vector3.Distance(monsterTransform.position, playerTransform.position) <= catchRadius)
                {
                    // Transition to IdleState
                    GameManager.Instance.ChangeToCaughtScene();
                   // float idleDuration = 5f; // Idle for 5 seconds
                   // IMonsterState idleState = new IdleState(monsterTransform.GetComponent<MonsterController>(), idleDuration);
                   // monsterTransform.GetComponent<MonsterController>().TransitionToState(idleState);
                }
            }
            else
            {
                timeSinceLastSeenPlayer += Time.deltaTime;
                if (timeSinceLastSeenPlayer > timeToLosePlayer)
                {
                    Debug.Log("I Quit, loser");
                    // Switch back to RoamingState
                    monsterTransform.GetComponent<MonsterController>().TransitionToRoaming();

                }
            }

        }
        
        // Check for losing the player, transitioning to another state, etc.
    }
    private bool CanSeePlayer()
    {
        Vector3 directionToPlayer = playerTransform.position - monsterTransform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= viewRange)
        {
            float angleToPlayer = Vector3.Angle(monsterTransform.forward, directionToPlayer);

            if (angleToPlayer <= fieldOfView / 2)
            {
                if (!Physics.Raycast(monsterTransform.position, directionToPlayer.normalized, distanceToPlayer, obstacleMask))
                {
                    return true; // Player is visible
                }
            }
        }

        return false; // Player is not visible
    }
    public void OnExitState()
    {
        // Cleanup if needed when exiting the chasing state
    }

    private IEnumerator ReactionDelay()
    {
        // Wait for the specified reaction time
        yield return new WaitForSeconds(reactionTime);
        isChasing = true;
    }
}


public class IdleState : IMonsterState
{
    private MonsterController monsterController;
    private float idleDuration;
    private float idleTimer;
   // private int lastPrintedTime = -1;

    public IdleState(MonsterController monsterController, float idleDuration)
    {
        this.monsterController = monsterController;
        this.idleDuration = idleDuration;
    }

    public void OnEnterState()
    {
       // idleTimer = idleDuration;
        monsterController.agent.isStopped = true; // Stop the monster's movement
        Debug.Log("Entering IdleState");
        //Debug.Log("Entering Idle State. Countdown: " + Mathf.CeilToInt(idleTimer) + " seconds");

    }

    public void OnUpdateState()
    {
        /*idleTimer -= Time.deltaTime;

        // Print the countdown in whole seconds, only when it changes
        int remainingTime = Mathf.CeilToInt(idleTimer);
        if (remainingTime != lastPrintedTime && remainingTime > 0)
        {
            Debug.Log("Idle State Countdown: " + remainingTime + " seconds remaining");
            lastPrintedTime = remainingTime;
        }
        
        if (idleTimer <= 0)
        {
            monsterController.agent.isStopped = false; // Resume movement
            monsterController.TransitionToRoaming(); // Transition back to RoamingState
            Debug.Log("Exiting Idle State, transitioning to Roaming State.");
        }*/
    }

    public void OnExitState()
    {
        Debug.Log("Exiting IdleState");
        // Ensure the monster can move when exiting the idle state
        monsterController.agent.isStopped = false;
    }
}
