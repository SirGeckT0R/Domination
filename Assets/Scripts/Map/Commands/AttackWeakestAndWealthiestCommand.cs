using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.Players;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/AttackWeakestAndWelthiestCommand")]
    public class AttackWeakestAndWealthiestCommand : Command
    {
        public Player attackTarget;
        public Player player;
        public List<Player> others;
        public County county;

        private CountyManager _countyManager;

        public override void Execute()
        {
            if (attackTarget == null)
            {
                Debug.Log("Not a valid attack target");
            }

            Debug.Log("Attacking this player: " + attackTarget);

            var rand = Random.Range(0, 10);
            if (rand > 5)
            {
                player.Warriors -= 5;
                player.Money -= 10;

                attackTarget.Warriors -= 10;
                attackTarget.Money += 10;

                Debug.Log("Fight was lost");
            }
            else
            {
                if (player is AIPlayer)
                {
                    county = _countyManager.ChooseCounty(player, attackTarget);
                }

                player.Warriors -= 5;
                player.Money += 10;

                attackTarget.Warriors -= 10;
                attackTarget.Money -= 10;

                _countyManager.TryChangeOwners(player.Id, county);
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
            this.attackTarget = context.WarTargetInfo.AttackTarget;
            this.county = context.WarTargetInfo.County;
            this._countyManager = context.CountyManager;
        }
    }
}
