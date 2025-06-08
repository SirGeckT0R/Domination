using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/HostileCurveConsideration")]
    public class DeclareWarConsideration : CurveConsideration
    {
        public List<RelationEventType> ignoreEvents;

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
                                    && !ShouldIgnore(relEvent, current)
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

        private bool ShouldIgnore(RelationEvent relEvent, Player current)
        {
            return ignoreEvents.Contains(relEvent.EventType) || (relEvent.EventType == RelationEventType.SentPact && relEvent.SenderId != current.Id);
        }
    }
}
