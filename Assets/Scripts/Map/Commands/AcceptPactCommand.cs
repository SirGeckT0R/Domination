using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.UI.GameLog;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/AcceptPactCommand")]
    public class AcceptPactCommand : Command, IIrreversible
    {
        public string RecieverName { get; private set; }
        public CreatePactEvent PactEvent { get; private set; }
        public List<RelationEvent> RelationEvents{ get; private set; }

        public override MessageDto Execute()
        {
            if (PactEvent == null)
            {
                Debug.Log("Not a valid pact event");

                return null;
            }

            var message = new MessageDto { Player = RecieverName, Message = $"Принял пакт" };
            RelationEvents.Remove(PactEvent);
            var acceptPact = new RelationEvent(PactEvent.SenderId, PactEvent.RecieverId, RelationEventType.AcceptedPact, 3);
            RelationEvents.Add(acceptPact);

            Debug.Log("Accepting this pact: " + PactEvent);

            return message;
        }

        public override void Undo()
        {
        }


        public override void UpdateContext(Context context)
        {
        }

        public void UpdateContext(string PlayerName, CreatePactEvent pactEvent, List<RelationEvent> relationEvents)
        {
            RecieverName = PlayerName;
            RelationEvents = relationEvents;
            PactEvent = pactEvent;
        }
    }
}
