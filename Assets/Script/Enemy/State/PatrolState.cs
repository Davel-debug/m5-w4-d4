using UnityEngine;

public class PatrolState : EnemyState
{
    private float idleTimer = 0f;
    public PatrolState(EnemyController enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        if (enemy.waypoints.Length > 1)
            enemy.agent.SetDestination(enemy.waypoints[enemy.currentWaypointIndex].position);
    }

    public override void Update()
    {
        if (enemy.waypoints.Length == 0) return;

        if (enemy.enemyType == EnemyController.EnemyType.Idle || enemy.waypoints.Length == 1)
            IdleLookAtWaypoints();
        else
            Patrol();
    }

    private void IdleLookAtWaypoints()
    {
        // Rimane fermo all’origine
        enemy.agent.ResetPath();
        enemy.transform.position = enemy.idleOrigin;

        if (enemy.waypoints.Length == 0) return;// niente altri waypoint da guardare

        idleTimer += Time.deltaTime;
        if (idleTimer >= enemy.idleRotationInterval)
        {
            // Salta il primo waypoint se è l’origine
            enemy.currentWaypointIndex = (enemy.currentWaypointIndex + 1) % enemy.waypoints.Length;
            if (enemy.currentWaypointIndex == 0) enemy.currentWaypointIndex = 1;
            idleTimer = 0f;
        }

        Vector3 targetDir = enemy.waypoints[enemy.currentWaypointIndex].position - enemy.transform.position;
        targetDir.y = 0; // mantieni l’asse y costante
        if (targetDir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(targetDir);
            enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, lookRot, enemy.idleRotationSpeed * Time.deltaTime);
        }

        if (enemy.fov.visibleTarget != null)
            stateMachine.ChangeState(new ChaseState(enemy, stateMachine));
    }
    
    private void Patrol()
    {
        if (!enemy.agent.pathPending && enemy.agent.remainingDistance < enemy.waypointTolerance)
        {
            enemy.currentWaypointIndex = (enemy.currentWaypointIndex + 1) % enemy.waypoints.Length;
            enemy.agent.SetDestination(enemy.waypoints[enemy.currentWaypointIndex].position);
        }

        if (enemy.fov.visibleTarget != null)
            stateMachine.ChangeState(new ChaseState(enemy, stateMachine));
    }
}
