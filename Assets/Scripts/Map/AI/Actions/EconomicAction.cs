using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Actions
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/EconomicAction")]
    public class EconomicAction : AIAction
    {
        public override void Execute(Context context)
        {
            context.CurrentPlayer.Money += 5;
            context.CurrentPlayer.Warriors -= 1;
        }
    }
}
