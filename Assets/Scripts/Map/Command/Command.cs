using Assets.Scripts.Map.AI.Considerations;
using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    public abstract class Command : ScriptableObject
    {
        public string targetTag;
        public Consideration consideration;
        public float CalculateUtility(Context context) => consideration.Evaluate(context);

        public abstract void UpdateContext(Context context);
        public abstract void Execute();
        public abstract void Undo();
    }
}
