using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.Players;

namespace Assets.Scripts.Map.AI.Events
{
    public class CreatePactEvent : RelationEvent
    {
        public CreatePactEvent(Player player1, Player player2, int turnsLeft) : base(player1, player2, RelationEventType.SentPact, turnsLeft)
        {
        }
    }
}
