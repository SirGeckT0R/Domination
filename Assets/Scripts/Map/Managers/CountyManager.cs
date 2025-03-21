using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Players;
using ModestTree;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.Managers
{
    public class CountyManager : MonoBehaviour
    {
        [field: SerializeField] public List<County> AllCounties { get; private set; } = new List<County>();

        [field: SerializeField] public Dictionary<ushort, List<County>> CountyOwners { get; private set; } = new Dictionary<ushort, List<County>>();

        [field: SerializeField] public byte MaxEconomicLevel { get; } = 5;
        [field: SerializeField] public byte MaxMilitaryLevel { get; } = 5;
        [field: SerializeField] public int PriceForMilitaryUpgrade { get; } = 15;

        private void Start()
        {
            var countiesObjects = FindObjectsByType<County>(FindObjectsSortMode.None);
            Array.ForEach(countiesObjects, county =>
            {
                AllCounties.Add(county);

                var belongsTo = county.BelongsTo;
                var doesPlayerListExist = CountyOwners.ContainsKey(belongsTo);
                if (!doesPlayerListExist)
                {
                    CountyOwners[belongsTo] = new List<County>();
                }

                CountyOwners[belongsTo].Add(county);

                return;
            });
        }

        public bool TryChangeOwners(ushort newOwner, County county)
        {
            var doesOwnerExist = CountyOwners.ContainsKey(newOwner);
            if (!doesOwnerExist)
            {
                return false;
            }

            CountyOwners[county.BelongsTo].Remove(county);
            county.BelongsTo = newOwner;
            CountyOwners[newOwner].Add(county);

            return true;
        }

        public CountyStats GetCountiesTotalStats(ushort playerId)
        {
            var totalEconomicLevel = 0;
            var totalMilitaryLevel = 0;
            var playerCounties = CountyOwners[playerId];
            foreach (var county in playerCounties)
            {
                totalEconomicLevel += county.EconomicLevel;
                totalMilitaryLevel += county.MilitaryLevel;
            }

            int maxPossible = MaxEconomicLevel * playerCounties.Count;
            var canUpgradeEconomic = totalEconomicLevel < maxPossible;

            int maxPossibleMilitary = MaxMilitaryLevel * playerCounties.Count;
            var canUpgradeMilitary = totalMilitaryLevel < maxPossibleMilitary;

            return new CountyStats(totalEconomicLevel, totalMilitaryLevel, canUpgradeEconomic, canUpgradeMilitary);
        }

        public County ChooseCounty(Player current, Player attackTarget)
        {
            var targetCounties = CountyOwners[attackTarget.Id];
            var chosenCounty = UnityEngine.Random.Range(0, targetCounties.Count);

            return targetCounties[chosenCounty];
        }

        public County ChooseCountyForEconomicUpgrade(ushort playerId)
        {
            var playerCounties = CountyOwners[playerId];
            return playerCounties.FirstOrDefault(county => county.EconomicLevel < MaxEconomicLevel);
        }

        public County ChooseCountyForMilitaryUpgrade(ushort playerId)
        {
            var playerCounties = CountyOwners[playerId];
            return playerCounties.FirstOrDefault(county => county.MilitaryLevel < MaxMilitaryLevel);
        }

        public List<ushort> PlayersWhoLost()
        {
            return CountyOwners.Where(playerCounties => playerCounties.Value.Count == 0)
                .Select(playerCounties => playerCounties.Key)
                .ToList();
        }
    }
}
