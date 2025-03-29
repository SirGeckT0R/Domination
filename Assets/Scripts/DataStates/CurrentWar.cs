using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Battleground/CurrentWar")]
    public class CurrentWar : ScriptableObject
    {
        public WarInfo CurrentWarInfo { get; set; }
        public WarResult CurrentWarResult { get; set; }

        private void Awake()
        {
            CurrentWarInfo = null;
            CurrentWarResult = null;
        }
    }
}
