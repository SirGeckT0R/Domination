using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.Players;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/AttackWeakestAndWelthiestCommand")]
    public class AttackWeakestAndWealthiestCommand : Command
    {
        public Player attackTarget;
        public Player player;
        public Player[] others;

        public override void Execute()
        {
            if (attackTarget == null)
            {
                Debug.Log("Not a valid attack target");
            }

            Debug.Log("Attacking this player: " + attackTarget);

            var rand = Random.Range(0,10);
            if (rand > 5)
            {
                player.Warriors -= 20;
                player.Money -= 30;

                attackTarget.Warriors -= 5;
                attackTarget.Money += 30;

                Debug.Log("Fight was lost");
            }
            else
            {
                player.Warriors -= 5;
                player.Money += 30;

                attackTarget.Warriors -= 20;
                attackTarget.Money -= 30;

                Debug.Log("Fight was won");
            }
        }

        public override void Undo()
        {
        }


        public override void UpdateContext(Context context)
        {
            this.player = context.CurrentPlayer;
            this.others = context.OtherPlayers;
            this.attackTarget = context.AttackTarget;
        }
    }
}
