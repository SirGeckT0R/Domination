using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Players;
using Assets.Scripts.Map.UI.GameLog;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/SendPactCommand")]
    public class SendPactCommand : Command
    {
        public Player PactTarget { get; private set; }
        public Player Player { get; private set; }
        public List<RelationEvent> RelationEvents { get; private set; }

        private List<RelationEvent> _prevRelationEvents;
        private List<CreatePactEvent> _prevTargetPactCommands;

        public override MessageDto Execute()
        {
            var isTargetNull = PactTarget == null;
            var arePlayersInvolved = RelationEvents.Any(relEvent => relEvent.ArePlayersInvolved(Player.Id, PactTarget.Id));
            if (isTargetNull || arePlayersInvolved)
            {
                Debug.Log("Not a valid pact target");

                return null;
            }

            _prevRelationEvents = new List<RelationEvent>(RelationEvents);
            _prevTargetPactCommands = new List<CreatePactEvent>(PactTarget.PactCommands);

            var pactEvent = new CreatePactEvent(Player.Id, PactTarget.Id, 3);
            PactTarget.PactCommands.Add(pactEvent);
            RelationEvents.Add(pactEvent);

            var message = new MessageDto { Player = Player.Name, Message = $"Sent pact to {PactTarget.Name}" };
            Debug.Log("Creating a pact with this player: " + PactTarget.Name);

            return message;
        }

        public override void Undo()
        {
            PactTarget.PactCommands.Clear();
            PactTarget.PactCommands.AddRange(_prevTargetPactCommands);

            RelationEvents.Clear();
            RelationEvents.AddRange(_prevRelationEvents);
        }

        public override void UpdateContext(Context context)
        {
            Player = context.CurrentPlayer;
            PactTarget = context.PactTarget;
            RelationEvents = context.RelationEvents;
        }

        public void UpdateContext(Player current, Player other, List<RelationEvent> relations)
        {
            Player = current;
            PactTarget = other;
            RelationEvents = relations;
        }
    }
}
