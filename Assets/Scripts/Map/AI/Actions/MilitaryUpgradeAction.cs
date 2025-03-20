using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Actions
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/WarfareAction")]
    public class MilitaryUpgradeAction : AIAction
    {
        public override void Execute(Context context)
        {
            var county = context.CountyManager.ChooseCountyForMilitaryUpgrade(context.CurrentPlayer.Id);
            if (county == null)
            {
                return;
            }

            county.IncrementBuildingLevel(Counties.BuildingType.Military);
            context.CurrentPlayer.Money -= 15;
        }
    }
}
