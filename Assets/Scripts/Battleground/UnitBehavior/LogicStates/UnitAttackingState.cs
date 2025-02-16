using UnityEngine;

public class UnitAttackingState : State
{
    private Camera _camera;
    private AttackController _attackController;
    private UnitMovement _unitMovement;
    private Animator _animator;

    private float _attackTimer;

    public UnitAttackingState(Unit unit, IStateMachine stateMachine) : base(unit, stateMachine)
    {
    }

    public UnitAttackingState(IStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _camera = Camera.main;
        _attackController = unit.GetComponent<AttackController>();
        _unitMovement = unit.GetComponent<UnitMovement>();
        _animator = unit.GetComponent<Animator>();

        _animator.SetBool("IsAttacking", true);
    }

    public override void Exit()
    {
        _animator.SetBool("IsAttacking", false);
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

        var shouldStopAttacking = attackTarget == null || _unitMovement.IsCommandedToMove;

        if (shouldStopAttacking)
        {
            stateMachine.ChangeState(unit.IdleState);

            return;
        }

        if (_attackTimer <= 0)
        {
            _attackController.AttackTarget();
            _attackTimer = 1f / _attackController.AttacksPerSecond;
        }
        else
        {
            _attackTimer -= Time.deltaTime;
        }

        float distanceToTarget = Vector3.Distance(attackTarget.transform.position, unit.transform.position);

        shouldStopAttacking = attackTarget == null || distanceToTarget > _attackController.StopAttackingDistance;

        if (shouldStopAttacking)
        {
            stateMachine.ChangeState(unit.IdleState);

            return;
        }
    }
}

