using Assets.Scripts.Battleground.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Battleground.BattleGoals
{
    public class BattleGoal : MonoBehaviour
    {
        public UnityEvent<GoalAchievedInfo> OnGoalAchieved;
        [field: SerializeField] public BattleOpponent AchievedBy { get; private set; }
    }
}
