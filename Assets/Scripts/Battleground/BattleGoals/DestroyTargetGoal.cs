using UnityEngine;

namespace Assets.Scripts.Battleground.BattleGoals
{
    [RequireComponent(typeof(Health))]
    public class DestroyTargetGoal : BattleGoal
    {
        private void Awake()
        {
            var unitHealth = GetComponent<Health>();
            unitHealth.OnDeath.AddListener(HandleTargetDeath);
        }

        private void HandleTargetDeath() => OnGoalAchieved?.Invoke();
    }
}
