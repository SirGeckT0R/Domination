using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.Players;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/AttackCommand")]
    public class AttackCommand : Command, IUndoable
    {
        public Player AttackTarget { get; private set; }
        public Player Player { get; private set; }
        public County County { get; private set; }

        private CountyManager _countyManager;

        public override void Execute()
        {
            if (AttackTarget == null)
            {
                Debug.Log("Not a valid attack target");
            }

            Debug.Log("Attacking this player: " + AttackTarget);

            var rand = Random.Range(0, 10);
            if (rand > 5)
            {
                Player.Warriors -= 5;
                Player.Money -= 10;

                AttackTarget.Warriors -= 10;
                AttackTarget.Money += 10;

                Debug.Log("Fight was lost");
            }
            else
            {
                if (Player is AIPlayer)
                {
                    County = _countyManager.ChooseCounty(Player, AttackTarget);
                }

                Player.Warriors -= 5;
                Player.Money += 10;

                AttackTarget.Warriors -= 10;
                AttackTarget.Money -= 10;

                Debug.Log("Fight was won");
                _countyManager.TryChangeOwners(Player.Id, County);
                //HandleCountySwap();
            }
        }

        //private void HandleCountySwap()
        //{
        //    var isSuccessful = _countyManager.TryChangeOwners(player.Id, county);
        //    if (!isSuccessful)
        //    {
        //        return;
        //    }

        //    if (player is HumanPlayer newOwner)
        //    {
        //        newOwner.AddCountyListener(county);
        //    }

        //    if (attackTarget is HumanPlayer oldOwner)
        //    {
        //        oldOwner.RemoveCountyListener(county);
        //    }
        //}

        public override void Undo()
        {
        }


        public override void UpdateContext(Context context)
        {
            Player = context.CurrentPlayer;
            AttackTarget = context.WarTargetInfo.AttackTarget;
            County = context.WarTargetInfo.County;
            _countyManager = context.CountyManager;
        }

        public void UpdateContext(County county, Player player, CountyManager countyManager, Player attackTarget)
        {
            Player = player;
            County = county;
            AttackTarget = attackTarget;
            _countyManager = countyManager;
        }
    }
}
