using Assets.Scripts.Map.Players;
using Assets.Scripts.Map.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private List<PlayerIcon> _playerIcons;
        [SerializeField] private GameObject _playerIconPrefab;
        [SerializeField] private GameObject _iconsView;
        public IInteractable CurrentPlayer { get; private set;  }
        public List<Player> OtherPlayers { get; private set; }
        public void HandleCountyInteraction(CountyInteractionInfo countyInteractionInfo) => CurrentPlayer?.HandleCountyInteraction(countyInteractionInfo);

        public void DisplayPlayerHUD(IInteractable currentPlayer, List<Player> others)
        {
            CurrentPlayer = currentPlayer;
            OtherPlayers = others;

            _playerIcons.Clear();
            foreach (var player in OtherPlayers)
            {
                var playerIcon = Instantiate(_playerIconPrefab, _iconsView.transform).GetComponent<PlayerIcon>();
                playerIcon.Player = player;
                _playerIcons.Add(playerIcon);
            }
        }
    }
}
