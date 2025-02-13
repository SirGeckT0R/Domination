using UnityEngine;

public class UnitIdleState : State
{
    private AttackController _attackController;
    private Animator _animator;

    public UnitIdleState(Unit unit, IStateMachine stateMachine) : base(unit, stateMachine)
    {
    }

    public override void Enter()
    {
        _attackController = unit.GetComponent<AttackController>();
        _animator = unit.GetComponent<Animator>();

        _animator.SetBool("IsIdle", true);
    }

    public override void Exit()
    {
        _animator.SetBool("IsIdle", false);
    }

    public override void HandleInput()
    {
    }

    public override void Update()
    {
        if (_attackController.AttackTarget != null)
        {
            stateMachine.ChangeState(unit.FollowingState);
        }
    }
}
