using UnityEngine;

namespace Assets.Scripts.Battleground.UnitBehavior.LogicStates
{
    public class UnitAttackingState : State
    {
        protected Camera _camera;
        protected AttackController _attackController;
        protected Animator _animator;

        protected float _attackTimer;

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
            if (ShouldStopAttacking())
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
        }

        protected virtual bool ShouldStopAttacking()
        {
            return _attackController.Target == null;
        }
    }
}