using Assets.Scripts.Battleground.UnitBehavior.Units;

namespace Assets.Scripts.Battleground.UnitBehavior.LogicStates
{
    public class WallAttackingState : UnitAttackingState
    {
        public WallAttackingState(Wall unit, IStateMachine stateMachine) : base(unit, stateMachine)
        {
        }

        public WallAttackingState(IStateMachine stateMachine) : base(stateMachine)
        {
        }
    }
}
