
using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/MilitaryUpgradeConsideration")]
    public class MilitaryUpgradeConsideration : CurveConsideration
    {
        public float priceForMilitaryUpgrade;
        public float optimalMilitaryLevel;

        public override float Evaluate(Context context)
        {
            var stats = context.CountyManager.GetCountiesTotalStats(context.CurrentPlayer.Id);

            if (!stats.CanUpgradeMilitary || context.CurrentPlayer.Money < priceForMilitaryUpgrade)
            {
                return 0f;
            }

            var inputValue = 1f - stats.TotalMilitaryLevel / optimalMilitaryLevel;

            var utility = curve.Evaluate(inputValue);

            return utility;
        }
    }
}
