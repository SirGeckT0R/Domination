using Assets.Scripts.Battleground.UnitBehavior.Units;

namespace Assets.Scripts.Battleground.UnitBehavior.LogicStates
{
    public class KnightFollowingState : UnitFollowingState
    {
        public KnightFollowingState(Knight unit, IStateMachine stateMachine) : base(unit, stateMachine)
        {
        }

        public KnightFollowingState(IStateMachine stateMachine) : base(stateMachine) { }
    }
}
