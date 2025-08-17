using UnityEngine;

public class ReturnState : EnemyState
{
    public ReturnState(EnemyController enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        if (enemy.waypoints.Length > 0)
            enemy.agent.SetDestination(enemy.waypoints[enemy.currentWaypointIndex].position);
    }

    public override void Update()
    {
        if (enemy.waypoints.Length > 0 && !enemy.agent.pathPending && enemy.agent.remainingDistance < enemy.waypointTolerance)
        {
            stateMachine.ChangeState(new PatrolState(enemy, stateMachine));
        }

        if (enemy.fov.visibleTarget != null)
        {
            stateMachine.ChangeState(new ChaseState(enemy, stateMachine));
        }
    }
}
