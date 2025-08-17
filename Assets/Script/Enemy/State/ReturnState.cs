using UnityEngine;

public class ReturnState : EnemyState
{
    public ReturnState(EnemyController enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }


    public override void Enter()
    {
        enemy.agent.SetDestination(enemy.idleOrigin);
    }

    public override void Update()
    {
        if (!enemy.agent.pathPending && enemy.agent.remainingDistance < enemy.waypointTolerance)
        {
            stateMachine.ChangeState(new PatrolState(enemy, stateMachine)); // torna idle
        }

        if (enemy.fov.visibleTarget != null)
        {
            stateMachine.ChangeState(new ChaseState(enemy, stateMachine));
        }
    }

}