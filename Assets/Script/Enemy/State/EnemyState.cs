using UnityEngine;

public abstract class EnemyState
{
    protected EnemyController enemy;
    protected EnemyStateMachine stateMachine;

    protected EnemyState(EnemyController enemy, EnemyStateMachine stateMachine)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
