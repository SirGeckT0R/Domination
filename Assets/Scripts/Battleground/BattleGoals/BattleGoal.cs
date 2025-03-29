using Assets.Scripts.Battleground.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Battleground.BattleGoals
{
    public class BattleGoal : MonoBehaviour
    {
        public UnityEvent<GoalAchievedInfo> OnGoalAchieved;
        [field: SerializeField] public BattleOpponent AchievedBy { get; set; }

        public virtual void Initialize()
        {
            if (OnGoalAchieved == null)
            {
                OnGoalAchieved = new UnityEvent<GoalAchievedInfo>();
            }
        }
    }
}
