using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Players;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/AcceptPactCommand")]
    public class AcceptPactCommand : Command
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

            Debug.Log("Accepting pact from this player: " + pactTarget);
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
