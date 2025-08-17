using UnityEngine;

public class ChaseState : EnemyState
{
    private float lostPlayerTimer;

    public ChaseState(EnemyController enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        enemy.agent.speed = enemy.chaseSpeed;
        lostPlayerTimer = 0f;
    }

    public override void Update()
    {
        if (enemy.fov.visibleTarget != null)
        {
            // Aggiorna destinazione verso player
            enemy.lastPlayerPosition = enemy.fov.visibleTarget.position;
            enemy.agent.SetDestination(enemy.lastPlayerPosition);
            lostPlayerTimer = 0f;
        }
        else
        {
            // Player non visibile ? vai verso ultima posizione
            enemy.agent.SetDestination(enemy.player.transform.position);
            lostPlayerTimer += Time.deltaTime;

            if (lostPlayerTimer >= enemy.timeToLosePlayer) // 3 secondi
            {
                stateMachine.ChangeState(new SearchState(enemy, stateMachine));
            }
        }
    }
}
