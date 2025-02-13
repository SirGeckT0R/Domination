using UnityEngine;
using UnityEngine.AI;

public class UnitFollowingState : State
{
    private AttackController _attackController;
    private NavMeshAgent _navMeshAgent;
    private UnitMovement _unitMovement;
    private Animator _animator;

    public UnitFollowingState(Unit unit, IStateMachine stateMachine) : base(unit, stateMachine)
    {
    }

    public override void Enter()
    {
        _attackController = unit.GetComponent<AttackController>();
        _navMeshAgent = unit.GetComponent<NavMeshAgent>();
        _unitMovement = unit.GetComponent<UnitMovement>();
        _animator = unit.GetComponent<Animator>();

        _animator.SetBool("IsFollowing", true);
    }

    public override void Exit()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath();

        _animator.SetBool("IsFollowing", false);
    }

    public override void HandleInput()
    {
    }

    public override void Update()
    {
        var attackTarget = _attackController.AttackTarget;

        if (attackTarget == null)
        {
            stateMachine.ChangeState(unit.IdleState);

            return;
        }

        if (!_unitMovement.IsCommandedToMove)
        {
            _navMeshAgent.SetDestination(attackTarget.transform.position);
            //refactor
            unit.transform.LookAt(attackTarget.transform);

            var distanceToTarget = Vector3.Distance(attackTarget.transform.position, unit.transform.position);

            if (distanceToTarget < _attackController.AttackingDistance)
            {
                stateMachine.ChangeState(unit.AttackingState); 

                return;
            }
        }
    }
}

