using UnityEngine;
using Zenject;

public class Unit : MonoBehaviour
{
    private IStateMachine _stateMachine;
    private UnitSelectionManager _selectionManager;

    public UnitIdleState IdleState { get; private set; }
    public UnitFollowingState FollowingState { get; private set; }
    public UnitAttackingState AttackingState { get; private set; }

    public bool IsSelected { get; set; }

    [Inject]
    public void Construct(IStateMachine stateMachine, UnitSelectionManager unitSelectionManager)
    {
        _stateMachine = stateMachine;
        _selectionManager = unitSelectionManager;

        IdleState = new UnitIdleState(this, _stateMachine);
        FollowingState = new UnitFollowingState(this, _stateMachine);
        AttackingState = new UnitAttackingState(this, _stateMachine);
    }

    private void Start()
    {
        _selectionManager.AddUnit(gameObject);

        _stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        if (IsSelected)
        {
            _stateMachine.CurrentState.HandleInput();
        }

        _stateMachine.CurrentState.Update();
    }


    private void OnDestroy()
    {
        _selectionManager.RemoveUnit(gameObject);
    }
}
