using UnityEngine;

public class UnitFollowingState : State
{
    private Camera _camera;
    private AttackController _attackController;
    private UnitMovement _unitMovement;
    private Animator _animator;

    public UnitFollowingState(Unit unit, IStateMachine stateMachine) : base(unit, stateMachine)
    {
    }

    public UnitFollowingState(IStateMachine stateMachine): base(stateMachine) { }

    public override void Enter()
    {
        _camera = Camera.main;
        _attackController = unit.GetComponent<AttackController>();
        _unitMovement = unit.GetComponent<UnitMovement>();
        _animator = unit.GetComponent<Animator>();

        _animator.SetBool("IsFollowing", true);
    }

    public override void Exit()
    {
        if(!_unitMovement.IsCommandedToMove)
        {
            _unitMovement.StopMovement();
        }

        _animator.SetBool("IsFollowing", false);
    }

    public override void HandleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _attackController.Attackable))
            {
                _unitMovement.StopMovement();
                var target = hit.transform.GetComponent<Unit>();

                _attackController.SetTarget(target);
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _unitMovement.Ground))
            {
                _unitMovement.CommandUnit(hit);
            }
        }
    }


    public override void Update()
    {
        var attackTarget = _attackController.Target;

        if (attackTarget == null)
        {
            stateMachine.ChangeState(unit.IdleState);

            return;
        }

        if (!_unitMovement.IsCommandedToMove)
        {
            _unitMovement.MoveUnit(attackTarget.transform);

            var distanceToTarget = Vector3.Distance(attackTarget.transform.position, unit.transform.position);

            if (distanceToTarget < _attackController.AttackingDistance)
            {
                stateMachine.ChangeState(unit.AttackingState); 

                return;
            }
        }
    }
}

