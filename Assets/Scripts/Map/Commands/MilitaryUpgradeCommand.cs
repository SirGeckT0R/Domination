using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.Players;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/MilitaryUpgradeCommand")]
    public class MilitaryUpgradeCommand : Command
    {
        public int militaryUpgradePrice = 15;
        private Player player;
        private CountyManager countyManager;
        public override void UpdateContext(Context context)
        {
            this.player = context.CurrentPlayer;
            this.countyManager = context.CountyManager;
        }

        public override void Execute()
        {
            var county = countyManager.ChooseCountyForMilitaryUpgrade(player.Id);
            if (county == null)
            {
                return;
            }

            county.IncrementBuildingLevel(Counties.BuildingType.Military);
            player.Money -= militaryUpgradePrice;
            Debug.Log($"Executing military upgrade action with parameters");
        }

        //public void Undo()
        //{
        //    Debug.Log($"Undoing relations action with parameters {losses}");
        //}

        public override void Undo()
        {
            //player.Warriors -= losses;
            //player.Money += 1;
            //Debug.Log($"Undoing relations action with parameters {losses}");
        }
    }
}