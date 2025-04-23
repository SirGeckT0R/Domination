public interface IStateMachine
{
    State CurrentState { get; }
    public bool HasExited { get; }

    void Initialize(State startingState);

    void ChangeState(State newState);
    public void Exit();
}
