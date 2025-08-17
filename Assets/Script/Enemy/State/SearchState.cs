using UnityEngine;

public class SearchState : EnemyState
{
    private float searchTimer = 0f;
    private int direction = 1;

    public SearchState(EnemyController enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        enemy.agent.ResetPath(); // si ferma
        searchTimer = 0f;
    }

    public override void Update()
    {
        searchTimer += Time.deltaTime;

        // rotazione alternata dx/sx
        float angle = Mathf.Sin(searchTimer * 5f) * 45f; // oscilla tra -45° e +45°
        Quaternion rot = Quaternion.Euler(0, enemy.idleOriginRotation.eulerAngles.y + angle, 0);
        enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, rot, enemy.turnSpeed * Time.deltaTime * 100);

        if (enemy.fov.visibleTarget != null)
        {
            stateMachine.ChangeState(new ChaseState(enemy, stateMachine));
            return;
        }

        if (searchTimer > enemy.searchTime) // dopo 2 secondi di "cerca"
        {
            stateMachine.ChangeState(new ReturnState(enemy, stateMachine));
        }
    }
}
