using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    private IMonsterState currentState;
    private NavMeshAgent agent;

    // References to state instances
    private RoamingState roamingState;
    private ChasingState chasingState;
    private IdleState idleState;

    void Awake()
    {
        Transform playerTransform = null; // change to player
        
        agent = GetComponent<NavMeshAgent>();
        // Initialize states with required references
        roamingState = new RoamingState(agent, transform);
        chasingState = new ChasingState(agent, transform, playerTransform); // playerTransform needs to be defined
        idleState = new IdleState(agent);

        // Start in idle state
        TransitionToState(idleState);
    }

    void Update()
    {
        currentState?.OnUpdateState();
    }

    public void TransitionToState(IMonsterState newState)
    {
        currentState?.OnExitState();
        currentState = newState;
        currentState.OnEnterState();
    }

    // Call this method to transition to chasing
    public void StartChasing()
    {
        TransitionToState(chasingState);
    }

    // ... Add other methods as needed for state transitions
}
