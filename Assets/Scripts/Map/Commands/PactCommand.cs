using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Players;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/PactCommand")]
    public class PactCommand : Command
    {
        public string pact;
        public Player player;
        public Player[] others;

        public override void Execute()
        {
            Debug.Log("Creating a pact with this player: " + pact);
            var pactTarget = others.FirstOrDefault(p => p.Name.Equals(pact));

            if (pactTarget == null)
            {
                Debug.Log("Not a valid pact target");
            }

            Debug.Log("Here i will send a pact request");
        }

        public override void Undo()
        {
        }


        public override void UpdateContext(Context context)
        {
            this.player = context.CurrentPlayer;
            this.others = context.OtherPlayers;
            this.pact = context.AttackTarget;
        }
    }
}
