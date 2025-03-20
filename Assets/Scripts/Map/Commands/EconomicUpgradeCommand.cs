using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.Players;
using UnityEngine;
namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/EconomicUpgradeCommand")]
    public class EconomicUpgradeCommand : Command
    {
        public int money;
        private Player player;
        private CountyManager countyManager;
        public override void UpdateContext(Context context)
        {
            this.player = context.CurrentPlayer;
            this.countyManager = context.CountyManager;
        }

        //public void Execute()
        //{
        //    player.Money += money;
        //    player.Warriors -= money;
        //    Debug.Log($"Executing an economic action with parameters {money}");
        //}

        public override void Execute()
        {
            var county = countyManager.ChooseCountyForEconomicUpgrade(player.Id);
            if (county == null)
            {
                return;
            }

            county.IncrementBuildingLevel(Counties.BuildingType.Economic);
            Debug.Log($"Executing an economic upgrade action with parameters");
        }

        //public void Undo()
        //{
        //    Debug.Log($"Undoing an economic action with parameters {money}");
        //}

        public override void Undo()
        {
            //player.Money -= money;
            //player.Warriors += 1;
            //Debug.Log($"Undoing an economic action with parameters {money}");
        }
    }
}
