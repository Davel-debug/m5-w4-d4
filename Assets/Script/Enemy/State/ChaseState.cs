using UnityEngine;

public class ChaseState : EnemyState
{
    private float losePlayerTimer = 0f;

    public ChaseState(EnemyController enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        losePlayerTimer = 0f;
        enemy.agent.speed = enemy.chaseSpeed; // assicurati di avere una velocità maggiore
    }

    public override void Update()
    {
        Transform target = enemy.fov.visibleTarget;

        if (target != null)
        {
            enemy.lastPlayerPosition = target.position;
            losePlayerTimer = 0f;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
        }

        // Gira verso la destinazione più velocemente
        Vector3 dir = (enemy.lastPlayerPosition - enemy.transform.position).normalized;
        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, enemy.turnSpeed * Time.deltaTime);
        }

        // Setta la destinazione dell'agente NavMesh
        enemy.agent.SetDestination(enemy.lastPlayerPosition);

        // Se perde il player, rimani nella posizione per un po’ e cerca intorno
        if (losePlayerTimer >= enemy.timeToLosePlayer)
        {
            stateMachine.ChangeState(new ReturnState(enemy, stateMachine));
        }
    }
}
