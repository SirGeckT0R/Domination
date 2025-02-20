using Assets.Scripts.Map.AI.Contexts;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/PactCurveConsideration")]
    public class PactCurveConsideration : Consideration
    {
        public AnimationCurve curve;
        public string contextKey;
        public float keyImportance;

        public override float Evaluate(Context context)
        {
            var current = context.CurrentPlayer;
            var other = context.OtherPlayers;
            var maxUtility = 0f;
            //rename
            var nameOfAttackTarget = "";

            var utility = -1f;
            foreach (var player in other)
            {
                var commonEvent = context.RelationEvents.FirstOrDefault(relEvent => relEvent.ArePlayersInvolved(current, player));

                if (commonEvent != null)
                {
                    return 0f;
                }

                var inputValue = player.Warriors / (current.Warriors * keyImportance);
                var clamped = Mathf.Clamp01(inputValue);

                utility = curve.Evaluate(clamped);
                if (utility > maxUtility)
                {
                    maxUtility = utility;
                    //rename
                    nameOfAttackTarget = player.Name;
                }
            }

            //rename
            context.AttackTarget = nameOfAttackTarget;

            return maxUtility;
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
