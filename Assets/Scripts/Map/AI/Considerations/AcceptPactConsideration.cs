using Assets.Scripts.Map.AI.Contexts;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/AcceptPactConsideration")]
    public class AcceptPactConsideration : CurveConsideration
    {
        public float pactAcceptance;

        public override float Evaluate(Context context)
        {
            var pact = context.CurrentPact;
            if (pact == null)
            {
                return 0f;
            }
            var sender = context.OtherPlayers.FirstOrDefault(player => pact.SenderId == player.Id);
            float result = sender.Warriors * pactAcceptance / context.CurrentPlayer.Warriors;
            float clamped = Mathf.Clamp01(result);

            context.PactTarget = sender;

            return curve.Evaluate(clamped);
        }
    }
}
