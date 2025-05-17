using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/EconomicUpgradeConsideration")]
    public class EconomicUpgradeConsideration : CurveConsideration
    {
        public float keyImportance;
        public float priceForMilitaryUpgrade;

        public override float Evaluate(Context context)
        {
            var priceForMilitaryUpgrade = context.CountyManager.PriceForMilitaryUpgrade;
            var stats = context.CountyManager.GetCountiesTotalStats(context.CurrentPlayer.Id);

            if (!stats.CanUpgradeEconomy)
            {
                return 0f;
            }

            var inputValue = stats.TotalEconomicLevel * keyImportance / priceForMilitaryUpgrade;

            var utility = curve.Evaluate(inputValue);

            return utility;
        }
    }
}

