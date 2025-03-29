using UnityEngine;

namespace Assets.Scripts.DataStates
{
    [CreateAssetMenu(menuName = "Map/CountyInfo")]
    public class CountyInfo : ScriptableObject
    {
        [field: SerializeField] public ushort Id { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public byte EconomicLevel { get; set; }
        [field: SerializeField] public byte MilitaryLevel { get; set; }
        [field: SerializeField] public ushort BelongsTo { get; set; }

        public void Initialize(byte economicLevel, byte militaryLevel, ushort belongsTo)
        {
            EconomicLevel = economicLevel;
            MilitaryLevel = militaryLevel;
            BelongsTo = belongsTo;
        }

        public void Initialize(CountyInfo countyInfo)
        {
            Name = countyInfo.Name;
            EconomicLevel = countyInfo.EconomicLevel;
            MilitaryLevel = countyInfo.MilitaryLevel;
            BelongsTo = countyInfo.BelongsTo;
        }

        public void Deconstruct(out string name, out byte economicLevel, out byte militaryLevel, out ushort belongsTo)
        {
            name = Name;
            economicLevel = EconomicLevel;
            militaryLevel = MilitaryLevel;
            belongsTo = BelongsTo;
        }
    }
}
