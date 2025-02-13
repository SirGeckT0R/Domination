public abstract class State
{
    protected Unit unit;
    protected IStateMachine stateMachine;

    protected State(Unit unit, IStateMachine stateMachine)
    {
        this.unit = unit;
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();

    public abstract void HandleInput();

    public abstract void Update();

    public abstract void Exit();
}
