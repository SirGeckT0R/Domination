using Assets.Scripts.Battleground.UnitBehavior.Units;
using UnityEngine;

namespace Assets.Scripts.Battleground.UnitBehavior.LogicStates
{
    public class KnightAttackingState : UnitAttackingState
    {
        protected UnitMovement _unitMovement;

        public KnightAttackingState(Knight unit, IStateMachine stateMachine) : base(unit, stateMachine)
        {
        }

        public KnightAttackingState(IStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _unitMovement = unit.GetComponent<UnitMovement>();
        }

        public override void Exit()
        {
            base.Exit();

            _unitMovement.StopMovement();
        }

        public override void HandleInput()
        {
            base.HandleInput();

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

        protected override bool ShouldStopAttacking()
        {
            var shouldStop = base.ShouldStopAttacking();
            if (shouldStop)
            {
                return true;
            }

            var attackTarget = _attackController.Target;
            float distanceToTarget = Vector3.Distance(attackTarget.transform.position, unit.transform.position);
            var isTooFar = distanceToTarget > _attackController.StopAttackingDistance;

            return _unitMovement.IsCommandedToMove || isTooFar;
        }
    }
}
