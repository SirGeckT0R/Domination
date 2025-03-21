using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Actions
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/EconomicAction")]
    public class EconomicUpgradeAction : AIAction
    {
        public override void Execute(Context context)
        {
            //var county = context.CountyManager.ChooseCountyForEconomicUpgrade(context.CurrentPlayer.Id);
            //if(county == null)
            //{
            //    return;
            //}

            //county.IncrementBuildingLevel(Counties.BuildingType.Economic);
        }
    }
}
