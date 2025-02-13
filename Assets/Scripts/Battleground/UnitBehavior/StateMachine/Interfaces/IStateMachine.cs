public interface IStateMachine
{
    State CurrentState { get; }

    void Initialize(State startingState);

    void ChangeState(State newState);
}
