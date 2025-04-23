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

        [field: SerializeField] public ushort Id { get; set; }
        [field: SerializeField] public string Name { get; set; }
        public byte EconomicLevel { 
            get => _economicLevel; 
            private set { _economicLevel = (byte)Mathf.Clamp(value, 0, _maxEconomicLevel); } 
        }
        public byte MilitaryLevel
        {
            get => _militaryLevel;
            private set { _militaryLevel = (byte)Mathf.Clamp(value, 0, _maxMilitaryLevel); }
        }

        [field: SerializeField] public ushort BelongsTo { get; set; }

        private byte _maxEconomicLevel;
        private byte _maxMilitaryLevel;

        private DataHolder _dataHolder;
        private BuildingVisualization _buildingVisualization;

        [Inject]
        public void Construct(CountyManager countyManager)
        {
            _maxEconomicLevel = countyManager.MaxEconomicLevel;
            _maxMilitaryLevel = countyManager.MaxMilitaryLevel;
            _dataHolder = DataHolder.Instance;
        }

        protected virtual void Awake()
        {
            _dataHolder = DataHolder.Instance;
            (Name, EconomicLevel, MilitaryLevel, BelongsTo) = _dataHolder.CountyInfos[Id];

            _buildingVisualization = GetComponent<BuildingVisualization>();
            _buildingVisualization.SetEconomicBuildingLevel(EconomicLevel);
            _buildingVisualization.SetMilitaryBuildingLevel(MilitaryLevel);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawMesh(GetComponent<MeshFilter>().sharedMesh, transform.position, transform.rotation, transform.localScale);
        }

        public void SetBuildingLevel(bool isEconomic, byte level)
        {
            if (isEconomic)
            {
                EconomicLevel = level;
                _buildingVisualization.SetEconomicBuildingLevel(EconomicLevel);
            }
            else
            {
                MilitaryLevel = level;
                _buildingVisualization.SetMilitaryBuildingLevel(MilitaryLevel);
            }
        }

        protected virtual void OnDestroy()
        {
            _dataHolder.CountyInfos[Id].Initialize(EconomicLevel, MilitaryLevel, BelongsTo);
        }
    }
}
