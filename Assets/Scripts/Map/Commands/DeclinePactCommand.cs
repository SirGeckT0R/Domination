using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.UI.GameLog;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map.Managers
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/DeclinePactCommand")]
    public class DeclinePactCommand : Command, IIrreversible
    {
        public string RecieverName { get; private set; }
        public CreatePactEvent PactEvent { get; private set; }
        public List<RelationEvent> RelationEvents { get; private set; }

        public override MessageDto Execute()
        {
            if (PactEvent == null)
            {
                Debug.Log("Not a valid pact event");
            }

            var message = new MessageDto { Player = RecieverName, Message = $"Отказал в заключении пакта" };
            RelationEvents.Remove(PactEvent);
            var acceptPact = new RelationEvent(PactEvent.SenderId, PactEvent.RecieverId, RelationEventType.DeniedPact, 3);
            RelationEvents.Add(acceptPact);

            Debug.Log("Decliining this pact: " + PactEvent);

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