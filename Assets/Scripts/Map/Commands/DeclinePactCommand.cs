using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Commands;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map.Managers
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/DeclinePactCommand")]
    public class DeclinePactCommand : Command, IUndoable
    {
        public CreatePactEvent PactEvent { get; private set; }
        public List<RelationEvent> RelationEvents { get; private set; }

        public override void Execute()
        {
            if (PactEvent == null)
            {
                Debug.Log("Not a valid pact event");
            }

            RelationEvents.Remove(PactEvent);
            var acceptPact = new RelationEvent(PactEvent.SenderId, PactEvent.RecieverId, RelationEventType.DeniedPact, 3);
            RelationEvents.Add(acceptPact);

            Debug.Log("Decliining this pact: " + PactEvent);
        }

        public override void Undo()
        {
        }


        public override void UpdateContext(Context context)
        {
        }

        public void UpdateContext(CreatePactEvent pactEvent, List<RelationEvent> relationEvents)
        {
            RelationEvents = relationEvents;
            PactEvent = pactEvent;
        }
    }
}