using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    public abstract class CurveConsideration : Consideration
    {
        public AnimationCurve curve;

        public void Reset()
        {
            curve = new AnimationCurve(
                new Keyframe(0f, 1f), // At normalized distance 0, utility is 1
                new Keyframe(1f, 0f)  // At normalized distance 1, utility is 0
            );
        }
    }
}
