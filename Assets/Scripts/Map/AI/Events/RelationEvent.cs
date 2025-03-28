using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.Players;

namespace Assets.Scripts.Map.AI.Events
{
    public class RelationEvent
    {
        public ushort SenderId { get; private set; }
        public ushort RecieverId { get; private set; }
        //public Player Sender { get; private set; }
        //public Player Reciever { get; private set; }
        public RelationEventType EventType { get; private set; }    
        public int TurnsLeft { get; private set; }

        public RelationEvent(ushort player1Id, ushort player2Id, RelationEventType eventType, int turnsLeft)
        {

            //Sender = player1; 
            SenderId = player1Id;
            //Reciever = player2;
            RecieverId = player2Id;
            EventType = eventType;
            TurnsLeft = turnsLeft;
        }

        public bool ArePlayersInvolved(ushort player1Id, ushort player2Id)
        {
            var isFirstPlayerInvolved = player1Id == SenderId || player1Id == RecieverId;
            var isSecondPlayerinvolved = player2Id == SenderId || player2Id == RecieverId;

            return isFirstPlayerInvolved && isSecondPlayerinvolved;
        }

        public bool UpdateDuration()
        {
            TurnsLeft--;

            return TurnsLeft <= 0;
        }
    }
}
