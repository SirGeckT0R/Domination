using Assets.Scripts.Map.AI.Contexts;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/AcceptPactConsideration")]
    public class AcceptPactConsideration : Consideration
    {
        public AnimationCurve curve;
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

        void Reset()
        {
            curve = new AnimationCurve(
                new Keyframe(0f, 1f), // At normalized distance 0, utility is 1
                new Keyframe(1f, 0f)  // At normalized distance 1, utility is 0
            );
        }
    }
}
