using UnityEngine;

namespace Assets.Scripts.Battleground.BattleGoals
{
    [RequireComponent(typeof(Army))]
    public class ArmyDefeatedGoal : BattleGoal
    {
        private void Awake()
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();

            var army = GetComponent<Army>();
            army.OnArmyDefeated.AddListener(HandleArmyDefeat);
        }

        private void HandleArmyDefeat() => OnGoalAchieved?.Invoke(new GoalAchievedInfo(this, AchievedBy));
    }
}
