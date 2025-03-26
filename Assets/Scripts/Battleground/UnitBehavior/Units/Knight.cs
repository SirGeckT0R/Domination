using Assets.Scripts.Battleground.UnitBehavior.LogicStates;
using Zenject;

namespace Assets.Scripts.Battleground.UnitBehavior.Units
{
    public class Knight : Unit
    {
        public UnitFollowingState FollowingState { get; protected set; }

        [Inject]
        public new void Construct(IStateMachine stateMachine, UnitSelectionManager unitSelectionManager)
        {
            base.Construct(stateMachine, unitSelectionManager);

            IdleState = new KnightIdleState(this, _stateMachine);
            FollowingState = new KnightFollowingState(this, _stateMachine);
            AttackingState = new KnightAttackingState(this, _stateMachine);
        }
    }
}
