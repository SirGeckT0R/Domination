using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Players;
using Assets.Scripts.Map.UI.GameLog;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/EconomicUpgradeCommand")]
    public class EconomicUpgradeCommand : Command
    {
        public Player Player { get; private set; }
        public County County { get; private set; }

        private byte _prevEconomicLevel = 0;

        public override void UpdateContext(Context context)
        {
            Player = context.CurrentPlayer;
            County = context.CountyManager.ChooseCountyForEconomicUpgrade(Player.Id);
        }

        public void UpdateContext(County county, Player player)
        {
            Player = player;
            County = county;
        }

        public override MessageDto Execute()
        {
            if (!IsValidCountyForUpgrade())
            {
                return null;
            }

            _prevEconomicLevel = County.EconomicLevel;
            County.SetBuildingLevel(true, (byte)(County.EconomicLevel + 1));
            
            var message = new MessageDto { Player = Player.Name, Message = $"Улучшил экономическое здание в {County.Name} до уровня {County.EconomicLevel}" };
            Debug.Log("Executing an economic upgrade action");

            return message;
        }

        public override void Undo()
        {
            if(County == null)
            {
                return;
            }

            County.SetBuildingLevel(true, _prevEconomicLevel);
            _prevEconomicLevel = 0;
            Debug.Log("Undoing an economic action");
        }

        private bool IsValidCountyForUpgrade()
        {
            return County != null && Player.Id == County.BelongsTo;
        }
    }
}
