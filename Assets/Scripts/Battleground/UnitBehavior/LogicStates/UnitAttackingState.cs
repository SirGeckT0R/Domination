using UnityEngine;

public class UnitAttackingState : State
{
    private AttackController _attackController;
    private Health _attackTargetHealth;
    private UnitMovement _unitMovement;
    private Animator _animator;

    private float _attackTimer;

    public UnitAttackingState(Unit unit, IStateMachine stateMachine) : base(unit, stateMachine)
    {
    }

    public override void Enter()
    {
        _attackController = unit.GetComponent<AttackController>();
        _attackTargetHealth = _attackController.Target.GetComponent<Health>();
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
            Attack();
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

    private void Attack()
    {
        var damage = _attackController.UnitDamage;

        if (_attackTargetHealth != null)
        {
            _attackTargetHealth.TakeDamage(damage);
        }

        SoundManager.Instance.PlayInfantryAttackSound();
    }
}

