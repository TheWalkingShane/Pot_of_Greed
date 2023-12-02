using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
	private IMonsterState currentState;
	public NavMeshAgent agent;
	// Singleton implementation for MonsterController
	public static MonsterController Instance { get; private set; }

	// States
	public RoamingState roamingState;
	public ChasingState chasingState;
	public IdleState idlestate;
	private bool allowStateChange = true;

	// You'll add more states here later

	// Parameters for the RoamingState
	[Tooltip("RoamingState")]
	private float roamRadius = 10f;
	[SerializeField] private float waitTimeBetweenRoam = 5f;
	[SerializeField] public float viewDistance = 15f; // How far the monster can see
	[SerializeField] public float viewAngle = 90f; // The field of view angle for line of sight
	[SerializeField] private LayerMask lineOfSightMask; // Layer mask to determine what objects block line of sight
	
	[Header("ChasingState")]
	[SerializeField] public float chaseSpeed = 1.7f;
	[SerializeField] public float reactionTime = 0.0f;
	[SerializeField] public float catchRadius = 1f ;
	[SerializeField]public float viewRange = 7f;
	[SerializeField]private float fieldOfView = 180f;
	[SerializeField]private LayerMask obstacleMask;
	
	public Transform playerTransform;

	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		
	}

	void Start()
	{
		// Create and enter the RoamingState
		roamingState = new RoamingState(agent, transform, playerTransform, roamRadius, waitTimeBetweenRoam, lineOfSightMask, viewDistance, viewAngle);
		chasingState = new ChasingState(agent, transform, playerTransform, chaseSpeed, reactionTime, catchRadius, viewRange, fieldOfView, obstacleMask);
		// Set your desired chase speed and reaction time
		TransitionToState(roamingState);


	}
	//===================================================================
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
	//===================================================================
	
	
	
	
	//===================================================================
	public void StartChasing()
	{
		TransitionToState(chasingState);
	}
	public void TransitionToRoaming()
	{
		// Assume roamingState is already initialized
		TransitionToState(roamingState);
	}

	public void EnterIdleState()
	{
		if (allowStateChange)
		{
			// Enter IdleState logic
			TransitionToState(new IdleState(this, 5f));
		}
		allowStateChange = false; // Prevent further state changes, this AllowStateChange is at the bottom
	}
	//===================================================================
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	public void AllowStateChange()
	{
		allowStateChange = true;
	}

	void OnDrawGizmos()
	{
		if (currentState is ChasingState chasingState)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, chasingState.CatchRadius);
		}
	}
}

