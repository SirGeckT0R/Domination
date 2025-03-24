using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Players;
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

        public override void Execute()
        {
            if (!IsValidCountyForUpgrade())
            {
                return;
            }

            _prevEconomicLevel = County.EconomicLevel;
            County.EconomicLevel++;

            Debug.Log("Executing an economic upgrade action");
        }

        public override void Undo()
        {
            if(County == null)
            {
                return;
            }

            County.EconomicLevel = _prevEconomicLevel;
            _prevEconomicLevel = 0;
            Debug.Log("Undoing an economic action");
        }

        private bool IsValidCountyForUpgrade()
        {
            return County != null && Player.Id == County.BelongsTo;
        }
    }
}
