using Assets.Scripts.Battleground.Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Battleground
{
    public class Army : MonoBehaviour
    {
        [field: SerializeField] public BattleOpponent BelongsTo { get; private set; }
        public List<Unit> Units { get; private set; } = new List<Unit>();
        public int WarriorsCount { get; private set; }

        public UnityEvent OnArmyDefeated;

        public void AddUnit(Unit unit)
        {
            Units.Add(unit);
            WarriorsCount++;
            unit.GetComponent<Health>().OnDeath.AddListener(HandleUnitDeath);
        }

        private void HandleUnitDeath()
        {
            WarriorsCount--;
            if(WarriorsCount <= 0)
            {
                OnArmyDefeated?.Invoke();
            }
        }
    }
}
