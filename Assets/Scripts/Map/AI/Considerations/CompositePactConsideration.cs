using Assets.Scripts.Map.AI.Contexts;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/CompositePactConsideration")]
    public class CompositePactConsideration : Consideration
    {
        [field:SerializeField] public AcceptPactConsideration pactConsideration { get; set; }
        [field: SerializeField] public HostileCurveConsideration attackConsideration { get; set; }

        public override float Evaluate(Context context)
        {
            var areOnlyTwoPlayersLeft = context.OtherPlayers.Count() < 2;
            if (areOnlyTwoPlayersLeft)
            {
                Debug.Log("Only two players left, no pacts accepted");

                return 0f;
            }

            float result = pactConsideration.Evaluate(context);

            float value = attackConsideration.Evaluate(context);

            if (context.WarTargetInfo.AttackTarget.Equals(context.PactTarget))
            {
                result -= value;
            }

            return Mathf.Clamp01(result);
        }
    }
}
