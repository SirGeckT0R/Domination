
using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Players;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/AcceptPactCommand")]
    public class AcceptPactCommand : Command
    {
        public string attack;
        public Player player;
        public Player[] others;

        public override void Execute()
        {
            Debug.Log("Attacking this player: " + attack);
            var attackTarget = others.FirstOrDefault(p => p.Name.Equals(attack));

            if (attackTarget == null)
            {
                Debug.Log("Not a valid attack target");
            }

            var rand = Random.Range(0, 10);
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
            this.attack = context.AttackTarget;
        }
    }
}
