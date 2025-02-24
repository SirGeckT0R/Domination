using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/HostileCurveConsideration")]
    public class HostileCurveConsideration: Consideration
    {
        public AnimationCurve curve;
        public List<RelationEventType> ignoreEvents;
        public float keyImportance;

        public override float Evaluate(Context context)
        {
            var current = context.CurrentPlayer;
            var other = context.OtherPlayers;
            var maxUtility = 0f;
            Player attackTarget = null;

            var utility = -1f;
            foreach (var player in other)
            {
                var commonEvent = context.RelationEvents.FirstOrDefault(relEvent => relEvent.ArePlayersInvolved(current, player)
                && !ignoreEvents.Contains(relEvent.EventType));

                if (commonEvent != null)
                {
                    continue;
                }

                var inputValue = Mathf.Clamp01(player.Money / keyImportance) - (float)player.Warriors / current.Warriors;
                var clamped = Mathf.Clamp01(inputValue);

                utility = curve.Evaluate(clamped);
                if (utility > maxUtility)
                {
                    maxUtility = utility;
                    attackTarget = player;
                }
            }

            context.AttackTarget = attackTarget;

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
