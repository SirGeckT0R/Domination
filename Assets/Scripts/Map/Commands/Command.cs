using Assets.Scripts.Map.AI.Considerations;
using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.UI.GameLog;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    public abstract class Command : ScriptableObject
    {
        public string targetTag;
        public Consideration consideration;
        public float CalculateUtility(Context context) => consideration.Evaluate(context);

        public abstract void UpdateContext(Context context);
        public abstract MessageDto Execute();
        public abstract void Undo();
    }
}
