using UnityEngine;

namespace Assets.Scripts.Battleground.BattleGoals
{
    [RequireComponent(typeof(Army))]
    public class ArmyDefeatedGoal : BattleGoal
    {
        private void Awake()
        {
            var army = GetComponent<Army>();
            army.OnArmyDefeated.AddListener(HandleArmyDefeat);
        }

        private void HandleArmyDefeat() => OnGoalAchieved?.Invoke(new GoalAchievedInfo(this, AchievedBy));
    }
}
