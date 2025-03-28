using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.Players;

namespace Assets.Scripts.Map.AI.Events
{
    public class CreatePactEvent : RelationEvent
    {
        public CreatePactEvent(ushort player1Id, ushort player2Id, int turnsLeft) : base(player1Id, player2Id, RelationEventType.SentPact, turnsLeft)
        {
        }
    }
}
