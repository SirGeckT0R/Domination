using UnityEngine;

public class UnitIdleState : State
{
    private Camera _camera;
    private AttackController _attackController;
    private Animator _animator;
    private UnitMovement _unitMovement;

    public UnitIdleState(Unit unit, IStateMachine stateMachine) : base(unit, stateMachine)
    {
    }

    public UnitIdleState(IStateMachine stateMachine): base(stateMachine)
    {
    }

    public override void Enter()
    {
        _camera = Camera.main;
        _attackController = unit.GetComponent<AttackController>();
        _animator = unit.GetComponent<Animator>(); 
        _unitMovement = unit.GetComponent<UnitMovement>();

        _animator.SetBool("IsIdle", true);
    }

    public override void Exit()
    {
        _animator.SetBool("IsIdle", false);
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
            else if(Physics.Raycast(ray, out hit, Mathf.Infinity, _unitMovement.Ground))
            {
                _unitMovement.CommandUnit(hit);
            }
        }
    }

    public override void Update()
    {
        if (_attackController.Target != null)
        {
            stateMachine.ChangeState(unit.FollowingState);
        }
    }
}
