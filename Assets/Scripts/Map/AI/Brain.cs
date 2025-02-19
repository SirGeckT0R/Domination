using Assets.Scripts.Map.AI.Actions;
using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Commands;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map.AI
{
    public class Brain : MonoBehaviour
    {
        public List<Command> actions;

        void Awake()
        {
            foreach (var action in actions)
            {
                //action.Initialize(Context);
            }
        }

        public Command FindAndProduceTheBestAction(Context context)
        {
            Command bestAction = null;
            float highestUtility = float.MinValue;

            foreach (var action in actions)
            {
                float utility = action.CalculateUtility(context);
                if (utility > highestUtility)
                {
                    highestUtility = utility;
                    bestAction = action;
                }
            }

            bestAction?.UpdateContext(context);

            Debug.Log("Best action: " + bestAction.GetType().Name + " with utility: " + highestUtility);

            return bestAction;
        }
    }
}
