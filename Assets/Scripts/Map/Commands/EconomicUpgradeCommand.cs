using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Players;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/EconomicUpgradeCommand")]
    public class EconomicUpgradeCommand : Command
    {
        private Player _player { get; set; }
        private County _county { get; set; }

        private byte _prevEconomicLevel = 0;

        public override void UpdateContext(Context context)
        {
            _player = context.CurrentPlayer;
            _county = context.CountyManager.ChooseCountyForEconomicUpgrade(_player.Id);
        }

        public void UpdateContext(County county, Player player)
        {
            _player = player;
            _county = county;
        }

        public override void Execute()
        {
            if (!IsValidCountyForUpgrade())
            {
                return;
            }

            _prevEconomicLevel = _county.EconomicLevel;
            _county.EconomicLevel++;

            Debug.Log("Executing an economic upgrade action");
        }

        public override void Undo()
        {
            if(_county == null)
            {
                return;
            }

            _county.EconomicLevel = _prevEconomicLevel;
            _prevEconomicLevel = 0;
            Debug.Log("Undoing an economic action");
        }

        private bool IsValidCountyForUpgrade()
        {
            return _county != null && _player.Id == _county.BelongsTo;
        }
    }
}
