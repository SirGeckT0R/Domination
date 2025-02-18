using Assets.Scripts.Map.AI.Actions;
using Assets.Scripts.Map.AI.Contexts;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map.AI
{
    public class Brain : MonoBehaviour
    {
        public List<AIAction> actions;
        public ContextData context;

        public Player player;

        void Awake()
        {
            player = GetComponent<Player>();

            foreach (var action in actions)
            {
                action.Initialize(context);
            }
        }

        public void FindAndProduceTheBestAction(ContextData data)
        {
            //UpdateContext();

            AIAction bestAction = null;
            float highestUtility = float.MinValue;

            foreach (var action in actions)
            {
                float utility = action.CalculateUtility(data);
                if (utility > highestUtility)
                {
                    highestUtility = utility;
                    bestAction = action;
                }
            }

            if (bestAction != null)
            {
                bestAction.Execute(data);

                Debug.Log("Best action: " + bestAction.GetType().Name + " with utility: " + highestUtility);
            }
        }

        void UpdateContext()
        {
            //context.SetData("health", health.normalizedHealth);
        }
    }
}
