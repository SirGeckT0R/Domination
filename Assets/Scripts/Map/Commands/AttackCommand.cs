using Assets.Scripts.Battleground.Enums;
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
        public bool HasWon { get; set; }
        public BattleOpponent Winner { get; set; }
        public BattleType BattleType { get; set; }
        public ushort AttackTargetId { get; private set; }
        public Player AttackTarget { get; private set; }
        public ushort PlayerId { get; private set; }
        public Player Player { get; private set; }
        public ushort CountyId { get; private set; }
        public County County { get; private set; }

        private CountyManager _countyManager;
        private WarResult _warResult;

        public override void Execute()
        {
            if (AttackTarget == null)
            {
                Debug.Log("Not a valid attack target");
            }

            Debug.Log("Attacking this player: " + AttackTarget);

            if (_warResult == null)
            {
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
                }

                return;
            }

            if (HasWon && BattleType == BattleType.Attack)
            {
                Player.Warriors = _warResult.RemainingPlayerWarriorsCount;
                AttackTarget.Warriors = _warResult.RemainingEnemyWarriorsCount;

                Player.Money += 10;

                AttackTarget.Money -= 10;

                Debug.Log("Fight was won");
                _countyManager.TryChangeOwners(Player.Id, County);
            }
            else if (HasWon && BattleType == BattleType.Defend)
            {
                Player.Warriors = _warResult.RemainingPlayerWarriorsCount;
                AttackTarget.Warriors = _warResult.RemainingEnemyWarriorsCount;

                Player.Money -= 10;

                AttackTarget.Money += 10;

                Debug.Log("Fight was won");
            }
            else if (!HasWon && BattleType == BattleType.Attack)
            {
                Player.Warriors = _warResult.RemainingPlayerWarriorsCount;
                AttackTarget.Warriors = _warResult.RemainingEnemyWarriorsCount;

                Player.Money -= 10;

                AttackTarget.Money += 10;

                Debug.Log("Fight was lost");
            }
            else if (!HasWon && BattleType == BattleType.Defend)
            {
                County = _countyManager.ChooseCounty(Player, AttackTarget);

                Player.Warriors = _warResult.RemainingPlayerWarriorsCount;
                AttackTarget.Warriors = _warResult.RemainingEnemyWarriorsCount;

                Player.Money += 10;

                AttackTarget.Money -= 10;

                Debug.Log("Fight was lost");
                _countyManager.TryChangeOwners(Player.Id, County);

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
            PlayerId = Player.Id;
            AttackTarget = context.WarTargetInfo.AttackTarget;
            AttackTargetId = AttackTarget.Id;
            County = context.WarTargetInfo.County;
            _countyManager = context.CountyManager;
            _warResult = null;
        }

        public void UpdateContext(County county, Player player, CountyManager countyManager, Player attackTarget)
        {
            Player = player;
            PlayerId = player.Id;

            County = county;
            CountyId = county ? county.Id : (ushort)0;

            AttackTarget = attackTarget;
            AttackTargetId = attackTarget.Id;

            _countyManager = countyManager;
            _warResult = null;
        }

        public void UpdateContext(County county, Player player, CountyManager countyManager, Player attackTarget, WarResult warResult)
        {
            Player = player;
            PlayerId = player.Id;

            County = county;
            CountyId = county ? county.Id : (ushort)0;

            AttackTarget = attackTarget;
            AttackTargetId = attackTarget.Id;

            _countyManager = countyManager;
            _warResult = warResult;
        }
    }
}
