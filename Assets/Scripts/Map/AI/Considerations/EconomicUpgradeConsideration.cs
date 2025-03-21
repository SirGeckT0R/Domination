using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/EconomicUpgradeConsideration")]
    public class EconomicUpgradeConsideration : Consideration
    {
        public AnimationCurve curve;
        public string contextKey;
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

        void Reset()
        {
            curve = new AnimationCurve(
                new Keyframe(0f, 1f), // At normalized distance 0, utility is 1
                new Keyframe(1f, 0f)  // At normalized distance 1, utility is 0
            );
        }
    }
}

