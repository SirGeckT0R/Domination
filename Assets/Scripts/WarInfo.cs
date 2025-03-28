using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Battleground/WarInfo")]
    public class WarInfo : ScriptableObject
    {
        [field: SerializeField] public BattleType BattleType { get; set; }
        [field: SerializeField] public int PlayerWarriorsCount { get; set; }
        [field: SerializeField] public int EnemyWarriorsCount { get; set; }

        public void Initialize(BattleType battleType, int playerWarriorsCount, int enemyWarriorsCount)
        {
            BattleType = battleType;
            PlayerWarriorsCount = playerWarriorsCount;
            EnemyWarriorsCount = enemyWarriorsCount;
        }
    }
}
