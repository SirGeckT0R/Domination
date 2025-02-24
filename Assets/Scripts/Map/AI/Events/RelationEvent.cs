using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.Players;

namespace Assets.Scripts.Map.AI.Events
{
    public class RelationEvent
    {
        public Player Sender { get; private set; }
        public Player Reciever { get; private set; }
        public RelationEventType EventType { get; private set; }
        public int TurnsLeft { get; private set; }

        public RelationEvent(Player player1, Player player2, RelationEventType eventType, int turnsLeft)
        {
            Sender = player1;
            Reciever = player2;
            EventType = eventType;
            TurnsLeft = turnsLeft;
        }

        public bool ArePlayersInvolved(Player player1, Player player2)
        {
            var isFirstPlayerInvolved = player1 == Sender || player1 == Reciever;
            var isSecondPlayerinvolved = player2 == Sender || player2 == Reciever;

            return isFirstPlayerInvolved && isSecondPlayerinvolved;
        }

        public bool UpdateDuration()
        {
            TurnsLeft--;

            return TurnsLeft <= 0;
        }
    }
}
