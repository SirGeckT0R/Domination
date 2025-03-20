using Assets.Scripts.Map.Managers;
using UnityEngine;

namespace Assets.Scripts.Map.Counties
{
    public class County : MonoBehaviour
    {
        public ushort Id { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public byte EconomicLevel { get; private set; }
        [field: SerializeField] public byte MilitaryLevel { get; set; }
        [field: SerializeField] public ushort BelongsTo { get; set; }

        private byte _maxEconomicLevel = CountyManager.MaxEconomicLevel;
        private byte _maxMilitaryLevel = CountyManager.MaxMilitaryLevel;

        public void IncrementBuildingLevel(BuildingType buildingType)
        {
            switch(buildingType)
            {
                case BuildingType.Economic:
                    IncrementEconomicLevel();

                    break;
                case BuildingType.Military:
                    IncrementMilitaryLevel();

                    break;
            }
        }

        private void IncrementEconomicLevel()
        {
            var newLevel = EconomicLevel + 1;
            EconomicLevel = (byte)Mathf.Clamp(newLevel, 0, _maxEconomicLevel);
        }

        private void IncrementMilitaryLevel()
        {
            var newLevel = MilitaryLevel + 1;
            MilitaryLevel = (byte)Mathf.Clamp(newLevel, 0, _maxMilitaryLevel);
        }
    }
}
