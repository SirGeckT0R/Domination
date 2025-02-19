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
        public int losses;
        private string attack;
        private Player player;
        private Player[] others;

        public override void Execute()
        {
            Debug.Log("Attacking this player: " + attack);

            var rand = Random.Range(0,10);
            if (rand > 5)
            {
                player.Warriors -= 20;
                player.Money -= 30;

                others.Where(p => p.Name.Equals(attack)).First().Warriors -= 5;
                others.Where(p => p.Name.Equals(attack)).First().Money += 30;
                Debug.Log("Fight was lost");
            }
            else
            {
                player.Warriors -= 5;
                player.Money += 30;

                others.Where(p => p.Name == attack).First().Warriors -= 20;
                others.Where(p => p.Name == attack).First().Money -= 30;

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
