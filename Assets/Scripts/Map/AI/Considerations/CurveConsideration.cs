using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/CurveConsideration")]
    public class CurveConsideration : Consideration
    {
        public AnimationCurve curve;
        public string contextKey;

        public override float Evaluate(ContextData context)
        {
            float inputValue;
            if(contextKey == "Warriors")
            {
                inputValue = (float)context.CurrentPlayer.Warriors / (float)100;
            }
            else
            {

                inputValue = (float)context.CurrentPlayer.Money / (float)200;
            }

            float utility = curve.Evaluate(inputValue);
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
