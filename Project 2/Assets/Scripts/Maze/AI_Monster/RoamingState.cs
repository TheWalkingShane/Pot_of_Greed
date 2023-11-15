using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingState : IMonsterState
{
    private readonly NavMeshAgent agent;
    private readonly Transform monster;

    private float roamingspeed = 4f;

    public RoamingState(NavMeshAgent agent, Transform monster)
    {
        this.agent = agent;
        this.monster = monster;
    }

    public void OnEnterState()
    {
        agent.speed = roamingspeed; // set roaming speed;
        // Set destination to a random point on the NavMesh
    }

    public void OnUpdateState()
    {
        // If reached destination, select a new random point
        // If player is detected, transition to chasing state
    }

    public void OnExitState()
    {
        // Cleanup if needed
    }
}

public class ChasingState : IMonsterState
{
    private readonly NavMeshAgent agent;
    private readonly Transform monster;
    private readonly Transform player;
    
    public float roamingspeed = 4f; // defualt roaming speed variable 

    public ChasingState(NavMeshAgent agent, Transform monster, Transform player)
    {
        this.agent = agent;
        this.monster = monster;
        this.player = player;
    }

    public void OnEnterState()
    {
        agent.isStopped = true;
        // Wait for 1 second (you can use a coroutine for this)
    }

    public void OnUpdateState()
    {
        // Chase the player
        // If player is out of sight, transition to roaming state
    }

    public void OnExitState()
    {
        agent.isStopped = false;
        agent.speed = roamingspeed;  // reset to roaming speed;
    }
}

public class IdleState : IMonsterState
{
    private readonly NavMeshAgent agent;

    public IdleState(NavMeshAgent agent)
    {
        this.agent = agent;
    }

    public void OnEnterState()
    {
        agent.isStopped = true;
        // Wait for 3 seconds (use a coroutine to transition to roaming)
    }

    public void OnUpdateState()
    {
        // Do nothing
    }

    public void OnExitState()
    {
        agent.isStopped = false;
    }
}
