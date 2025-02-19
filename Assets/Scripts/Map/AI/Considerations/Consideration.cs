using Assets.Scripts.Map.AI.Contexts;
using UnityEngine;

namespace Assets.Scripts.Map.AI.Considerations
{
    public abstract class Consideration : ScriptableObject
    {
        public abstract float Evaluate(Context context);
    }
}
