using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RoamingState : IMonsterState
{
    private readonly NavMeshAgent agent;
    private readonly Transform monster;
    private readonly float roamingspeed = 4f;
    private readonly float detectionRange = 10f;
    private bool playerDetected;

    public RoamingState(NavMeshAgent agent, Transform monster)
    {
        this.agent = agent;
        this.monster = monster;
    }

    public void OnEnterState()
    {
        agent.speed = roamingspeed; // Setting roaming speed
        SetRandomDestination(); // Setting destination to a random point on the NavMesh
        playerDetected = false; // setting player detection
    }

    public void OnUpdateState()
    {
        if (!playerDetected)
        {
            if (PlayerIsDetected())
            {
                playerDetected = true;
                // switch to chasing state
            }
            else if (ReachedDestination()) // If reached destination, select a new random point
            {
                SetRandomDestination();
            }
        }
    }

    public void OnExitState()
    {
        // Cleanup if needed
        playerDetected = false;
    }
    
    private void SetRandomDestination()
    {
        // Gets a random point on the NavMesh within a certain radius from the current position
        Vector3 randomPos = RandomNavSphere(monster.position, 10f, -1);

        agent.SetDestination(randomPos); // Setting the agent's destination to the random position
    }
    
    private bool ReachedDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true; // Agent reached its destination
                }
            }
        }
        return false;
    }
    
    private bool PlayerIsDetected()
    {
        // Your logic to detect the player within a certain range
        if (Vector3.Distance(monster.position, PlayerPosition()) <= detectionRange)
        {
            return true; // Player detected within the detection range
        }
        return false; // Player not detected within the range
    }
    
    private Vector3 PlayerPosition()
    {
        // Gets the player's position
        // return GameManager.Instance.GetPlayerPosition();
        return Vector3.zero;
    }

    
    // Helper method to get a random point on the NavMesh within a given radius
    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}

public class ChasingState : IMonsterState
{
    private readonly NavMeshAgent agent;
    private readonly Transform monster;
    private readonly Transform player;
    public float chasingSpeed = 8f; // defualt roaming speed variable

    public ChasingState(NavMeshAgent agent, Transform monster, Transform player)
    {
        this.agent = agent;
        this.monster = monster;
        this.player = player;
    }

    public void OnEnterState()
    {
        agent.speed = chasingSpeed; // setting chasing speed
        agent.isStopped = false; // start to move
    }

    public void OnUpdateState()
    {
        // Chase the player if player is out of sight, transition to roaming state
        if (player != null)
        {
            agent.SetDestination(player.position);
            // You can add additional logic here, like detecting if the player is out of sight
            // and transitioning back to roaming if necessary
        }
    }

    public void OnExitState()
    {
        // Clean up or reset any variables if needed when exiting the state
        agent.isStopped = true; // Stop the agent
        agent.speed = 0f; // Reset speed
    }
}

public class IdleState : IMonsterState
{
    private readonly NavMeshAgent agent;
    private readonly float idleDuration = 3f;
    private bool isIdling;
    private float idleTimer;

    public IdleState(NavMeshAgent agent)
    {
        this.agent = agent;
    }

    public void OnEnterState()
    {
        isIdling = true;
        idleTimer = 0f;
        agent.isStopped = true; // Stops the agent while idling
    }

    public void OnUpdateState()
    {
        if (isIdling)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleDuration)
            {
                isIdling = false;
                agent.isStopped = false; // Resume movement
                // Transition to another state (e.g., roaming or chasing)
            }
        }
    }

    public void OnExitState()
    {
        // Clean up or reset any variables if needed when exiting the state
        isIdling = false;
        idleTimer = 0f;
    }
}
