using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/Constant")]
    public class ConstantConsideration : Consideration
    {
        public float value;

        public override float Evaluate(Context context) => value;
    }
}
