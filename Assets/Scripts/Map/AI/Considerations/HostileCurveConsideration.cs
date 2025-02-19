using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/HostileCurveConsideration")]
    public class HostileCurveConsideration: Consideration
    {
        public AnimationCurve curve;
        public string contextKey;
        public float keyImportance;

        public override float Evaluate(Context context)
        {
            var current = context.CurrentPlayer;
            var other = context.OtherPlayers;
            var maxUtility = 0f;
            var nameOfAttackTarget = "";

            var utility = -1f;
            foreach (var player in other)
            {
                var inputValue = player.Money / keyImportance - (float)player.Warriors / current.Warriors;
                var clamped = Mathf.Clamp01(inputValue);

                utility = curve.Evaluate(clamped);
                if (utility > maxUtility)
                {
                    maxUtility = utility;
                    nameOfAttackTarget = player.Name;
                }
            }

            context.AttackTarget = nameOfAttackTarget;

            return maxUtility;
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
