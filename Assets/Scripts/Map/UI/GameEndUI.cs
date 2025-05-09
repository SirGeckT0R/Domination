using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.Players;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Map.UI
{
    public class GameEndUI : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _statsText;

        private CountyManager _countyManager;

        [Inject]
        public void Construct(CountyManager countyManager)
        {
            _countyManager = countyManager;
        }

        public void DisplayStats(IEnumerable<Player> players)
        {
            _panel.SetActive(true);

            var ordered = players.OrderByDescending(player => _countyManager.CountyOwners[player.Id].Count)
                .ThenByDescending(player => _countyManager.GetCountiesTotalStats(player.Id).TotalMilitaryLevel)
                .ThenByDescending(player => _countyManager.GetCountiesTotalStats(player.Id).TotalEconomicLevel)
                .ThenByDescending(player => player.Warriors)
                .ThenByDescending(player => player.Money)
                .ToList();

            _title.text = ordered[0] is HumanPlayer ? "Победа" : "Поражение";
            _statsText.text = string.Join("\n\n", ordered.Select(player => $"{player.Name} завершил(а) игру с {_countyManager.CountyOwners[player.Id].Count} графствами, общим уровнем военных зданий равным {_countyManager.GetCountiesTotalStats(player.Id).TotalMilitaryLevel}, общим уровнем экономических зданий равным {_countyManager.GetCountiesTotalStats(player.Id).TotalEconomicLevel}, {player.Warriors} воинами и {player.Money} единиц денег"));
        }
    }
}
