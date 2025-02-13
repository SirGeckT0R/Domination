public class StateMachine : IStateMachine
{
    public State CurrentState { get; protected set; }

    public void Initialize(State startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void ChangeState(State newState)
    {
        CurrentState.Exit();

        CurrentState = newState;
        newState.Enter();
    }
}

