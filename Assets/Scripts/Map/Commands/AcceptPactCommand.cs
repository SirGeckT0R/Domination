using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.AI.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/AcceptPactCommand")]
    public class AcceptPactCommand : Command, IUndoable
    {
        public CreatePactEvent PactEvent { get; private set; }
        public List<RelationEvent> RelationEvents{ get; private set; }

        public override void Execute()
        {
            if (PactEvent == null)
            {
                Debug.Log("Not a valid pact event");
            }

            RelationEvents.Remove(PactEvent);
            var acceptPact = new RelationEvent(PactEvent.Sender, PactEvent.Reciever, RelationEventType.AcceptedPact, 3);
            RelationEvents.Add(acceptPact);

            Debug.Log("Accepting this pact: " + PactEvent);
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
