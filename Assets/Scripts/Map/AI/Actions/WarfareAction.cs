
using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Actions
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/WarfareAction")]
    public class WarfareAction : AIAction
    {
        public override void Execute(Context context)
        {
            context.CurrentPlayer.Warriors += 5;
            context.CurrentPlayer.Money -= 1;
        }
    }
}
