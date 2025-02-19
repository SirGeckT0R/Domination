using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/CurveConsideration")]
    public class CurveConsideration : Consideration
    {
        public AnimationCurve curve;
        public string contextKey;
        public float keyImportance;

        public override float Evaluate(Context context)
        {
            var inputValue = context.GetData<int>(contextKey) / keyImportance;

            var utility = curve.Evaluate(inputValue);

            return Mathf.Clamp01(utility);
        }

        void Reset()
        {
            curve = new AnimationCurve(
                new Keyframe(0f, 1f), // At normalized distance 0, utility is 1
                new Keyframe(1f, 0f)  // At normalized distance 1, utility is 0
            );
        }
    }
}
