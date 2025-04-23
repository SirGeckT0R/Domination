using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/HostileCurveConsideration")]
    public class HostileCurveConsideration : Consideration
    {
        public AnimationCurve curve;
        public List<RelationEventType> ignoreEvents;
        public float keyImportance;
        public float warriorRatioThreshold;

        public override float Evaluate(Context context)
        {
            var current = context.CurrentPlayer;
            var other = context.OtherPlayers;
            var maxUtility = 0f;
            Player attackTarget = null;

            var utility = -1f;
            foreach (var player in other)
            {
                var commonEvent = context.RelationEvents
                    .FirstOrDefault(
                                    relEvent => relEvent.ArePlayersInvolved(current.Id, player.Id)
                                    && !ignoreEvents.Contains(relEvent.EventType)
                                    );

                if (commonEvent != null)
                {
                    continue;
                }

                
                var warriorsRatio = (float)player.Warriors / current.Warriors;

                var clamped = Mathf.Clamp01(warriorsRatio);

                utility = curve.Evaluate(clamped);
                if (utility > maxUtility)
                {
                    maxUtility = utility;
                    attackTarget = player;
                }
            }

            context.WarTargetInfo = new WarTargetInfo(attackTarget, null);

            return maxUtility;
        }

        void Reset()
        {
            curve = new AnimationCurve(
                new Keyframe(1f, 0f),
                new Keyframe(0f, 1f)
            );
        }
    }
}
