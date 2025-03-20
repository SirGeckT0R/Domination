
using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/MilitaryUpgradeConsideration")]
    public class MilitaryUpgradeConsideration : Consideration
    {
        public AnimationCurve curve;
        public string contextKey;
        public float keyImportance;
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

        void Reset()
        {
            curve = new AnimationCurve(
                new Keyframe(0f, 1f), // At normalized distance 0, utility is 1
                new Keyframe(1f, 0f)  // At normalized distance 1, utility is 0
            );
        }
    }
}
