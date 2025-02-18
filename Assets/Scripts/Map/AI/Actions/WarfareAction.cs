
using UnityEngine;

namespace Assets.Scripts.Map.AI.Actions
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/WarfareAction")]
    public class WarfareAction : AIAction
    {
        public override void Execute(ContextData context)
        {
            context.CurrentPlayer.Warriors += 5;
            context.CurrentPlayer.Money -= 1;
        }
    }
}
