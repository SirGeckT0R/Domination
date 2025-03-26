using UnityEngine;

namespace Assets.Scripts.Battleground.UnitBehavior.LogicStates
{
    public class UnitIdleState : State
    {
        protected Camera _camera;
        protected AttackController _attackController;
        protected Animator _animator;

        public UnitIdleState(Unit unit, IStateMachine stateMachine) : base(unit, stateMachine)
        {
        }

        public UnitIdleState(IStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _camera = Camera.main;
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
        }
    }
}