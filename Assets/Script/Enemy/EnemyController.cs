using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public enum EnemyType { Idle, Patrol }
    [Header("Type Settings")]
    public EnemyType enemyType = EnemyType.Patrol;

    [Header("Origin Settings")]
    public Vector3 idleOrigin;
    public Quaternion idleOriginRotation;

    [Header("References")]
    public Transform player;
    public FieldOfView fov;
    public NavMeshAgent agent;

    [Header("Animation")]
    public Animator animator;

    [Header("Patrol Settings")]
    public Transform[] waypoints;
    public int currentWaypointIndex = 0;
    public float waypointTolerance = 1f;
    public float patrolSpeed = 2f; 

    [Header("Idle Settings")]
    public float idleRotationSpeed = 30f;
    public float idleRotationInterval = 2f;

    [HideInInspector] public Vector3 lastPlayerPosition;

    [Header("Chase Settings")]
    public float timeToLosePlayer = 3f;
    public float chaseSpeed = 5f;
    public float turnSpeed = 5f;

    [Header("Vision Extras")]
    public float extraViewRadius = 5f; 

    [Header("Search Settings")]
    public float searchTime = 3f;
    public int searchOscillationCount = 2; //quante oscillazioni dx/sx
    public float searchRotationSpeed = 120f; //velocità rotazione ricerca

    private EnemyStateMachine stateMachine;



    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;

        if (enemyType == EnemyType.Idle)
            idleOrigin = transform.position;

        stateMachine = new EnemyStateMachine();
        idleOrigin = transform.position;
        idleOriginRotation = transform.rotation;
        stateMachine.ChangeState(new PatrolState(this, stateMachine));


    }

    private void Update()
    {
        stateMachine.Update();
        UpdateAnimatorParams();

    }
    void UpdateAnimatorParams()
    {
        if (animator == null) return;

        // Velocità lineare per Idle/Walk/Run
        float speed = (agent != null) ? agent.velocity.magnitude : 0f;
        animator.SetFloat("Speed", speed);

        /* Turn  calcolato dalla direzione del path vs forward
        if (agent != null && agent.desiredVelocity.sqrMagnitude > 0.01f)
        {
            float signedAngle = Vector3.SignedAngle(transform.forward, agent.desiredVelocity.normalized, Vector3.up);
            float turnNorm = Mathf.Clamp(signedAngle / 90f, -1f, 1f);
            animator.SetFloat("Turn", turnNorm);
        }
        else
        {
            animator.SetFloat("Turn", 0f);
        }*/
    }

}
