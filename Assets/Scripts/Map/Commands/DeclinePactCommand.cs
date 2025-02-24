using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.Players;
using UnityEngine;

namespace Assets.Scripts.Map.Managers
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/DeclinePactCommand")]
    public class DeclinePactCommand : Command
    {
        public Player pactTarget;
        public Player player;
        public Player[] others;

        public override void Execute()
        {
            if (pactTarget == null)
            {
                Debug.Log("Not a valid pact target");
            }

            Debug.Log("Declining pact from this player: " + pactTarget);
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