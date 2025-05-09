using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Players;
using Assets.Scripts.Map.UI.GameLog;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/MilitaryUpgradeCommand")]
    public class MilitaryUpgradeCommand : Command
    {
        //Change to ScriptableObject
        public int militaryUpgradePrice = 20;
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

        public override MessageDto Execute()
        {
            if (!IsValidForUpgrade())
            {
                return null;
            }

            _prevMilitaryLevel = County.MilitaryLevel;
            _prevMoney = Player.Money;

            County.SetBuildingLevel(false, (byte)(County.MilitaryLevel + 1));
            Player.Money -= militaryUpgradePrice;

            var message = new MessageDto { Player = Player.Name, Message = $"Улучшил военное здание в {County.Name} до уровня {County.MilitaryLevel}" };
            Debug.Log($"Executing military upgrade action");

            return message;
        }

        public override void Undo()
        {
            if (County == null)
            {
                return;
            }

            County.SetBuildingLevel(false, _prevMilitaryLevel);
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