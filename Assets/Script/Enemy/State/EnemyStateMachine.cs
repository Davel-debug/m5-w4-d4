using UnityEngine;

public class EnemyStateMachine
{
    private EnemyState currentState;

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;

        if (currentState != null)
            currentState.Enter();
    }

    public void Update()
    {
        if (currentState != null)
            currentState.Update();
    }
}
