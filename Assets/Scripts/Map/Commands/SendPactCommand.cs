using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/SendPactCommand")]
    public class SendPactCommand : Command
    {
        public Player pactTarget;
        public Player player;
        public List<Player> others;

        public override void Execute()
        {
            if (pactTarget == null)
            {
                Debug.Log("Not a valid pact target");
            }

            Debug.Log("Creating a pact with this player: " + pactTarget.Name);
        }

        public override void Undo()
        {
        }


        public override void UpdateContext(Context context)
        {
            this.player = context.CurrentPlayer;
            this.others = context.OtherPlayers;
            this.pactTarget = context.PactTarget;
        }
    }
}
