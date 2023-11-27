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
		GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        
		if(playerObject != null)
		{
			agent = GetComponent<NavMeshAgent>(); // Initialize states with required references
			if (agent != null && agent.isOnNavMesh)
			{
				Transform playerTransform = playerObject.transform;

				roamingState = new RoamingState(agent, transform);
				chasingState = new ChasingState(agent, transform, playerTransform); // playerTransform needs to be defined
				idleState = new IdleState(agent);

				TransitionToState(roamingState);
			}
		}
		else
		{
        	Debug.LogError("Player not found!");
    	}
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
    public void StartChasing()
    {
        TransitionToState(chasingState);
    }
    
    public void StartRoaming()
    {
	    TransitionToState(roamingState);
    }

    public void StartIdle()
    {
	    TransitionToState(idleState);
    }
}
