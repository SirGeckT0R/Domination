using UnityEngine;
using Assets.Scripts.Map.AI.Contexts;

namespace Assets.Scripts.Map.AI.Considerations
{
    public abstract class Consideration : ScriptableObject
    {
        public abstract float Evaluate(ContextData context);
    }
}
