using Assets.Scripts.Map.AI.Enums;

namespace Assets.Scripts.Map.AI.Events
{
    public class CreatePactEvent : RelationEvent
    {
        public string SenderName { get; set; }
        public CreatePactEvent(ushort player1Id, ushort player2Id, int turnsLeft, string senderName) : base(player1Id, player2Id, RelationEventType.SentPact, turnsLeft)
        {
            SenderName = senderName;
        }
    }
}
