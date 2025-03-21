using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.UI;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Assets.Scripts.Map.Counties
{
    public class County : MonoBehaviour
    {
        [SerializeField] private byte _economicLevel;
        [SerializeField] private byte _militaryLevel;

        public ushort Id { get; set; }
        [field: SerializeField] public string Name { get; set; }
        public byte EconomicLevel { 
            get => _economicLevel; 
            set { _economicLevel = (byte)Mathf.Clamp(value, 0, _maxEconomicLevel); } 
        }
        public byte MilitaryLevel
        {
            get => _militaryLevel;
            set { _militaryLevel = (byte)Mathf.Clamp(value, 0, _maxMilitaryLevel); }
        }

        [field: SerializeField] public ushort BelongsTo { get; set; }

        private byte _maxEconomicLevel;
        private byte _maxMilitaryLevel;

        public UnityEvent<CountyInteractionInfo> OnCountyInteractionEvent;

        [Inject]
        public void  Construct(CountyManager countyManager)
        {
            _maxEconomicLevel = countyManager.MaxEconomicLevel;
            _maxMilitaryLevel = countyManager.MaxMilitaryLevel;
        }

        //public void IncrementBuildingLevel(BuildingType buildingType)
        //{
        //    switch (buildingType)
        //    {
        //        case BuildingType.Economic:
        //            IncrementEconomicLevel();

        //            break;
        //        case BuildingType.Military:
        //            IncrementMilitaryLevel();

        //            break;
        //    }
        //}

        //private void IncrementEconomicLevel()
        //{
        //    var newLevel = EconomicLevel + 1;
        //    EconomicLevel = (byte)Mathf.Clamp(newLevel, 0, _maxEconomicLevel);
        //}

        //private void IncrementMilitaryLevel()
        //{
        //    var newLevel = MilitaryLevel + 1;
        //    MilitaryLevel = (byte)Mathf.Clamp(newLevel, 0, _maxMilitaryLevel);
        //}

        public void OnEconomicUpgradeClick()
        {
            var interactionInfo = new CountyInteractionInfo(this, CountyInteractionType.EconomicUpgrade);
            OnCountyInteractionEvent?.Invoke(interactionInfo);
        }

        public void OnMilitaryUpgradeClick()
        {
            var interactionInfo = new CountyInteractionInfo(this, CountyInteractionType.MilitaryUpgrade);
            OnCountyInteractionEvent?.Invoke(interactionInfo);
        }
    }
}
