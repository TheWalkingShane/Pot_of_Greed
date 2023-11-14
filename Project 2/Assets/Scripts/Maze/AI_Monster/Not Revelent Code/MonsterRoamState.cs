using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterRoamState : MonsterBaseState
{
    private NavMeshAgent navMeshAgent;
    private Vector3 randomDestination;
    private float roamRadius = 10f;
    private float roamTimer = 5f;

    public override void EnterState(MonsterStateManager monster){
        Debug.Log("Roaming State Started");
        navMeshAgent = monster.GetComponent<NavMeshAgent>();
        //StartCoroutine(Wander());
    }
    public override void UpdateState(MonsterStateManager monster){
        // Specific behavior during roaming if needed
        
    }
    public override void OnCollisionEnter(MonsterStateManager monster){
        // Handle collisions during roaming if needed
    }
    
    IEnumerator Wander()
    {
        while (true)
        {
            // Generate a random point within the specified radius
            randomDestination = Random.insideUnitSphere * roamRadius;

            // Set the destination on the NavMeshAgent
            navMeshAgent.SetDestination(randomDestination);

            // Wait for a random amount of time before generating a new destination
            yield return new WaitForSeconds(roamTimer);
        }
    }
}
