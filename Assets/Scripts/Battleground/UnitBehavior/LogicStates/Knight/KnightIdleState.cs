using Assets.Scripts.Battleground.UnitBehavior.Units;
using UnityEngine;

namespace Assets.Scripts.Battleground.UnitBehavior.LogicStates
{
    public class KnightIdleState : UnitIdleState
    {
        private UnitMovement _unitMovement;
        private Patrol _patrol;

        public KnightIdleState(Knight unit, IStateMachine stateMachine) : base(unit, stateMachine)
        {
        }

        public KnightIdleState(IStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _unitMovement = unit.GetComponent<UnitMovement>();
            _patrol = unit.GetComponent<Patrol>();

            EnablePatrol(true);
        }

        public override void Exit()
        {
            base.Exit();
            EnablePatrol(false);
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

                    if (!target.IsDead)
                    {
                        _attackController.SetTarget(target);
                    }
                }
                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _unitMovement.Ground))
                {
                    _unitMovement.CommandUnit(hit, unit.OrderOfSelection);
                }
            }
        }

        public override void Update()
        {
            if (_attackController.Target != null)
            {
                var knightUnit = unit as Knight;
                stateMachine.ChangeState(knightUnit.FollowingState);
            }
        }

        private void EnablePatrol(bool shouldActivate)
        {
            if (_patrol != null)
            {
                _patrol.enabled = shouldActivate;
            }
        }
    }
}
