using Assets.Scripts.Battleground.UnitBehavior.Units;

namespace Assets.Scripts.Battleground.UnitBehavior.LogicStates
{
    public class WallIdleState : UnitIdleState
    {
        public WallIdleState(Wall unit, IStateMachine stateMachine) : base(unit, stateMachine)
        {
        }

        public WallIdleState(IStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Update()
        {
            if (_attackController.Target != null && !unit.IsDead)
            {
                stateMachine.ChangeState(unit.AttackingState);
            }
        }
    }
}
