using UnityEngine;
using UnityEngine.AI;

public class DummyEnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    
    // Patroling
    public Vector3 walkPoint;
    private bool pointSet;
    public float pointRange;

    public float sightRange;
    public bool seenPlayer;

}
