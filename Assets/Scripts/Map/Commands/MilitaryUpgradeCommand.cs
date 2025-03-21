using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Players;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/MilitaryUpgradeCommand")]
    public class MilitaryUpgradeCommand : Command
    {
        public int militaryUpgradePrice = 15;
        private Player _player { get; set; }
        private County _county { get; set; }

        private byte _prevMilitaryLevel = 0;
        private int _prevMoney = 0;

        public override void UpdateContext(Context context)
        {
            _player = context.CurrentPlayer;
            _county = context.CountyManager.ChooseCountyForMilitaryUpgrade(_player.Id);
        }

        public void UpdateContext(County county, Player player)
        {
            _player = player;
            _county = county;
        }

        public override void Execute()
        {
            if (!IsValidForUpgrade())
            {
                return;
            }

            _prevMilitaryLevel = _county.MilitaryLevel;
            _prevMoney = _player.Money;

            _county.MilitaryLevel++;
            _player.Money -= militaryUpgradePrice;

            Debug.Log($"Executing military upgrade action");
        }

        public override void Undo()
        {
            if (_county == null)
            {
                return;
            }

            _county.MilitaryLevel = _prevMilitaryLevel;
            _player.Money = _prevMoney;

            _prevMilitaryLevel = 0;
            _prevMoney = 0;

            Debug.Log("Undoing an military action");
        }

        private bool IsValidForUpgrade()
        {
            return _county != null && _player.Id == _county.BelongsTo && _player.Money >= militaryUpgradePrice;
        }
    }
}