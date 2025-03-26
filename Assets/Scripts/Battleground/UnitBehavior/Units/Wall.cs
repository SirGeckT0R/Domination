using Assets.Scripts.Battleground.UnitBehavior.LogicStates;
using Zenject;

namespace Assets.Scripts.Battleground.UnitBehavior.Units
{
    public class Wall : Unit
    {
        [Inject]
        public new void Construct(IStateMachine stateMachine, UnitSelectionManager unitSelectionManager)
        {
            base.Construct(stateMachine, unitSelectionManager);

            IdleState = new WallIdleState(this, _stateMachine);
            AttackingState = new WallAttackingState(this, _stateMachine);
        }
    }
}
