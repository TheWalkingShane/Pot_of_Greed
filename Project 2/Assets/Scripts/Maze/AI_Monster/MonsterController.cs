using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    private IMonsterState currentState;
    private NavMeshAgent agent;

    // References to state instances
    [SerializeField]
    private static RoamingState roamingState;
    private static ChasingState chasingState;
    private static IdleState idleState;

    void Awake()
    {
		GameObject playerObject = GameObject.FindGameObjectWithTag("Player"); // sets the player object 
        
		if(playerObject != null) // sets up to get agents and states
		{
			agent = GetComponent<NavMeshAgent>(); // Initialize states with required references
			if (agent != null && agent.isOnNavMesh)
			{
				Transform playerTransform = playerObject.transform;

				roamingState = new RoamingState(agent, transform);
				chasingState = new ChasingState(agent, transform, playerTransform); // playerTransform needs to be defined
				idleState = new IdleState(agent);

				TransitionToState(roamingState); // originally was set to idle state but changed to roaming for testing
			}
		}
		else
		{
        	Debug.LogError("Player not found!");
    	}
    }

    void Update()
    {
        currentState?.OnUpdateState(); // updates to the current state
    }

    // changes and enters to a new state 
    public void TransitionToState(IMonsterState newState) 
    {
        currentState?.OnExitState();
        currentState = newState;
        currentState.OnEnterState();
    }
    public void StartChasing() // sets up for chasing state
    {
        TransitionToState(chasingState);
    }
    
    public void StartRoaming() // sets up for roaming state
    {
	    TransitionToState(roamingState);
    }

    public void StartIdle() // sets up for idle state
    {
	    TransitionToState(idleState);
    }
}
