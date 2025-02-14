using UnityEngine;

public class Unit : MonoBehaviour
{
    private IStateMachine _stateMachine;

    public UnitIdleState IdleState { get; private set; }
    public UnitFollowingState FollowingState { get; private set; }
    public UnitAttackingState AttackingState { get; private set; }

    private void Start()
    {
        UnitSelectionManager.Instance.AddUnit(gameObject);

        _stateMachine = new StateMachine();

        IdleState = new UnitIdleState(this, _stateMachine);
        FollowingState = new UnitFollowingState(this, _stateMachine);
        AttackingState = new UnitAttackingState(this, _stateMachine);

        _stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        _stateMachine.CurrentState.HandleInput();
        _stateMachine.CurrentState.Update();
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.RemoveUnit(gameObject);
    }
}
