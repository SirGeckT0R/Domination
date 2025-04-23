public class StateMachine : IStateMachine
{
    public State CurrentState { get; protected set; }
    public bool HasExited { get; protected set; }

    public void Initialize(State startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void ChangeState(State newState)
    {
        if (HasExited)
        {
            return;
        }

        CurrentState.Exit();

        CurrentState = newState;
        newState.Enter();
    }

    public void Exit()
    {
        HasExited = true;
    }
}

