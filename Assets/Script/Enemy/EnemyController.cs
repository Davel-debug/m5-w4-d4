using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float baseSpeed = 3.5f;           // Velocità base modificabile dall'Inspector
    public float difficultyMultiplier = 1f;  // Moltiplica solo il cono di visione

    [Header("References")]
    public EnemyVision vision;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (vision == null)
            vision = GetComponent<EnemyVision>();
    }

    private void Start()
    {
        ApplyDifficultySettings();
    }

    private void Update()
    {
        vision.FindVisibleTargets();

        if (vision.CanSeeTarget())
        {
            Vector3 targetPos = vision.visibleTarget.position;
            targetPos.y = transform.position.y; // blocca altezza
            agent.SetDestination(targetPos);
        }
        else
        {
            // Comportamento idle/pattuglia
            agent.ResetPath();
        }
    }

    public void ApplyDifficultySettings()
    {
        // La velocità si imposta solo da Inspector
        agent.speed = baseSpeed;

        // Il moltiplicatore agisce solo sul cono di visione
        vision.viewRadius *= difficultyMultiplier;
        vision.viewAngle *= difficultyMultiplier;
    }
}
