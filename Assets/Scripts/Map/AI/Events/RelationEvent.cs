using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.Players;

namespace Assets.Scripts.Map.AI.Events
{
    public class RelationEvent
    {
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public RelationEventType EventType { get; private set; }
        public int TurnsLeft { get; private set; }

        public RelationEvent(Player player1, Player player2, RelationEventType eventType, int turnsLeft)
        {
            Player1 = player1;
            Player2 = player2;
            EventType = eventType;
            TurnsLeft = turnsLeft;
        }

        public bool ArePlayersInvolved(Player player1, Player player2)
        {
            var isFirstPlayerInvolved = player1 == Player1 || player1 == Player2;
            var isSecondPlayerinvolved = player2 == Player1 || player2 == Player2;

            return isFirstPlayerInvolved && isSecondPlayerinvolved;
        }

        public bool UpdateDuration()
        {
            TurnsLeft--;

            return TurnsLeft <= 0;
        }
    }
}
