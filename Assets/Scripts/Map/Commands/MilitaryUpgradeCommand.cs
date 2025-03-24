using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Players;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/MilitaryUpgradeCommand")]
    public class MilitaryUpgradeCommand : Command
    {
        //Change to ScriptableObject
        public int militaryUpgradePrice = 15;
        public Player Player { get; private set; }
        public County County { get; private set; }

        private byte _prevMilitaryLevel = 0;
        private int _prevMoney = 0;

        public override void UpdateContext(Context context)
        {
            Player = context.CurrentPlayer;
            County = context.CountyManager.ChooseCountyForMilitaryUpgrade(Player.Id);
        }

        public void UpdateContext(County county, Player player)
        {
            Player = player;
            County = county;
        }

        public override void Execute()
        {
            if (!IsValidForUpgrade())
            {
                return;
            }

            _prevMilitaryLevel = County.MilitaryLevel;
            _prevMoney = Player.Money;

            County.MilitaryLevel++;
            Player.Money -= militaryUpgradePrice;

            Debug.Log($"Executing military upgrade action");
        }

        public override void Undo()
        {
            if (County == null)
            {
                return;
            }

            County.MilitaryLevel = _prevMilitaryLevel;
            Player.Money = _prevMoney;

            _prevMilitaryLevel = 0;
            _prevMoney = 0;

            Debug.Log("Undoing an military action");
        }

        private bool IsValidForUpgrade()
        {
            return County != null && Player.Id == County.BelongsTo && Player.Money >= militaryUpgradePrice;
        }
    }
}