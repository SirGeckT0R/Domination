using Assets.Scripts.Battleground.Enums;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Battleground/WarResult")]
    public class WarResult : ScriptableObject
    {
        [field: SerializeField] public BattleOpponent Winner { get; set; }
        [field: SerializeField] public int RemainingPlayerWarriorsCount { get; set; }
        [field: SerializeField] public int RemainingEnemyWarriorsCount { get; set; }

        public void Initialize(BattleOpponent winner, int remainingPlayerWarriorsCount, int remainingEnemyWarriorsCount)
        {
            Winner = winner;
            RemainingPlayerWarriorsCount = remainingPlayerWarriorsCount;
            RemainingEnemyWarriorsCount = remainingEnemyWarriorsCount;
        }
    }
}
