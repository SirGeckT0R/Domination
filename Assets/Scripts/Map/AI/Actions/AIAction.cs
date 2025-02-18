using Assets.Scripts.Map.AI.Considerations;
using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Actions
{
    public abstract class AIAction : ScriptableObject
    {
        public string targetTag;
        public Consideration consideration;

        public virtual void Initialize(ContextData context)
        {
            // Optional initialization logic
        }

        public float CalculateUtility(ContextData context) => consideration.Evaluate(context);

        public abstract void Execute(ContextData context);
    }
}
