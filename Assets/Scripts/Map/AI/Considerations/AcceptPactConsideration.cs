using Assets.Scripts.Map.AI.Contexts;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/AcceptPactConsideration")]
    public class AcceptPactConsideration : Consideration
    {
        public AnimationCurve curve;
        public string contextKey;
        public float keyValue;
        public float pactAcceptance;

        public override float Evaluate(Context context)
        {
            var current = context.CurrentPlayer;
            var pacts = current.PactCommands;

            var areOnlyTwoPlayersLeft = context.OtherPlayers.Count() < 2;
            if (areOnlyTwoPlayersLeft)
            {
                Debug.Log("Only two players left, no pacts accepted");

                return 0f;
            }

            foreach (var pact in pacts)
            {
                var other = pact.others.FirstOrDefault(p => p.Name.Equals(pact.pact));
                var inputValue = other.Warriors * keyValue / current.Warriors;
                var clamped = Mathf.Clamp01(inputValue);

                var utility = curve.Evaluate(clamped);

                var doesHaveLessWarriors = other.Warriors - current.Warriors < -20;
                if (doesHaveLessWarriors)
                {
                    Debug.Log("Decline pact");
                }else
                {
                    //add some more conditions

                    Debug.Log("Accept pact");
                }
            }

            return 0f;
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
