using UnityEngine;

public class SearchState : EnemyState
{
    private float searchTimer = 0f;

    public SearchState(EnemyController enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        enemy.agent.ResetPath();
        searchTimer = 0f;
    }

    public override void Update()
    {
        searchTimer += Time.deltaTime;

        // quante oscillazioni complete (dx+sx) fa nel searchTime
        float cycles = enemy.searchOscillationCount;
        float angle = Mathf.Sin(searchTimer * cycles * Mathf.PI / enemy.searchTime) * 45f;

        // direzione base = forward attuale dell’enemy
        Quaternion rot = Quaternion.Euler(0, enemy.transform.eulerAngles.y + angle, 0);
        enemy.transform.rotation = Quaternion.RotateTowards(
            enemy.transform.rotation,
            rot,
            enemy.searchRotationSpeed * Time.deltaTime
        );

        if (enemy.fov.visibleTarget != null)
        {
            stateMachine.ChangeState(new ChaseState(enemy, stateMachine));
            return;
        }

        if (searchTimer > enemy.searchTime)
        {
            stateMachine.ChangeState(new ReturnState(enemy, stateMachine));
        }
    }
}
