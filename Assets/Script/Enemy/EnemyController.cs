using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public enum EnemyType { Idle, Patrol }
    [Header("Type Settings")]
    public EnemyType enemyType = EnemyType.Patrol;
    [HideInInspector] public Vector3 idleOrigin;


    [Header("References")]
    public Transform player;
    public FieldOfView fov;
    public NavMeshAgent agent;

    [Header("Patrol Settings")]
    public Transform[] waypoints;
    public int currentWaypointIndex = 0;
    public float waypointTolerance = 1f;

    [Header("Idle Settings")]
    public float idleRotationSpeed = 30f;
    public float idleRotationInterval = 2f;

    [HideInInspector] public Vector3 lastPlayerPosition;

    [Header("Chase Settings")]
    public float timeToLosePlayer = 3f;
    public float chaseSpeed = 5f;
    public float turnSpeed = 5f;
    [Range(0f, 360f)] public float extraViewAngle = 30f;

    private EnemyStateMachine stateMachine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;

        if (enemyType == EnemyType.Idle)
            idleOrigin = transform.position;

        stateMachine = new EnemyStateMachine();
    }

    private void Start()
    {
        stateMachine.ChangeState(new PatrolState(this, stateMachine));
    }

    private void Update()
    {
        stateMachine.Update();
    }
}
