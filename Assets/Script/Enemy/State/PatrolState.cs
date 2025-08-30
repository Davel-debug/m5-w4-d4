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

    if (enemy.waypoints.Length == 0) return;

    // direzione verso il waypoint attuale
    Vector3 targetDir = enemy.waypoints[enemy.currentWaypointIndex].position - enemy.transform.position;
    targetDir.y = 0;

    if (targetDir != Vector3.zero)
    {
        Quaternion lookRot = Quaternion.LookRotation(targetDir);
        Quaternion prevRot = enemy.transform.rotation;

        // ruota fisicamente
        enemy.transform.rotation = Quaternion.RotateTowards(
            enemy.transform.rotation,
            lookRot,
            enemy.idleRotationSpeed * Time.deltaTime
        );

        // controlla quanto manca per essere allineato
        float angle = Quaternion.Angle(enemy.transform.rotation, lookRot);

        if (angle < 2f) // 2° di tolleranza
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= enemy.idleRotationInterval)
            {
                enemy.currentWaypointIndex = (enemy.currentWaypointIndex + 1) % enemy.waypoints.Length;
                idleTimer = 0f;
            }
        }
    }

    if (enemy.fov.visibleTarget != null)
        stateMachine.ChangeState(new ChaseState(enemy, stateMachine));
}




    private void Patrol()
    {
        enemy.agent.speed = enemy.patrolSpeed; 
        if (!enemy.agent.pathPending && enemy.agent.remainingDistance < enemy.waypointTolerance)
        {
            enemy.currentWaypointIndex = (enemy.currentWaypointIndex + 1) % enemy.waypoints.Length;
            enemy.agent.SetDestination(enemy.waypoints[enemy.currentWaypointIndex].position);
        }

        if (enemy.fov.visibleTarget != null)
            stateMachine.ChangeState(new ChaseState(enemy, stateMachine));
    }

}
