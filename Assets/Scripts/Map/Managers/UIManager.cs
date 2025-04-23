using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Players;
using Assets.Scripts.Map.UI;
using Assets.Scripts.Map.UI.GameLog;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Map.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private List<PlayerIcon> _playerIcons;
        [SerializeField] private GameObject _playerIconPrefab;
        [SerializeField] private GameObject _iconsView;
        [SerializeField] private PactView _pactView;
        [SerializeField] private GameLogView _gameLogView;
        [SerializeField] private GameObject _gameEndView;
        [SerializeField] private GameObject _attackedScreenView;

        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private TextMeshProUGUI _warriorsText;

        public HumanPlayer CurrentPlayer { get; private set; }
        public CreatePactEvent CurrentPact { get; private set; }
        public List<Player> OtherPlayers { get; private set; }

        private DiContainer _diContainer;

        [Inject]
        public void Construct(DiContainer container)
        {
            _diContainer = container;
        }

        public void HandleCountyInteraction(CountyInteractionInfo countyInteractionInfo) => CurrentPlayer?.HandleCountyInteraction(countyInteractionInfo);

        public void DisplayPlayerHUD(HumanPlayer currentPlayer, List<Player> others)
        {
            CurrentPlayer = currentPlayer;
            OtherPlayers = others;

            //CurrentPlayer.OnPactResolved.AddListener(HandlePactResolved);
            _playerIcons.ForEach(icon => Destroy(icon.gameObject));
            _playerIcons.Clear();
            foreach (var player in OtherPlayers)
            {
                var playerIcon = _diContainer.InstantiatePrefab(_playerIconPrefab, _iconsView.transform).GetComponent<PlayerIcon>();
                playerIcon.Player = player;
                _playerIcons.Add(playerIcon);
            }

            var pactCommands = CurrentPlayer.PactCommands;
            if (pactCommands.Count > 0)
            {
                CurrentPact = pactCommands[0];
                _pactView.gameObject.SetActive(true);
                _pactView.UpdateCurrentPact();
            }

            UpdatePlayerStats(currentPlayer);
        }

        public void UpdatePlayerStats(HumanPlayer player)
        {
            _moneyText.text = player.Money.ToString();
            _warriorsText.text = player.Warriors.ToString();
        }

        public void AddLogMessage(MessageDto message) => _gameLogView.AddLogMessage(message);

        public void HandleSendPactInteraction(Player player) => CurrentPlayer?.SendPactToPlayer(player);

        public void HandlePactResolved(bool isAccepted)
        {
            if (isAccepted)
            {
                CurrentPlayer.AcceptPact(CurrentPact);
            }
            else
            {
                CurrentPlayer.DeclinePact(CurrentPact);
            }

            var pactCommands = CurrentPlayer.PactCommands;
            if (pactCommands.Count > 0)
            {
                CurrentPact = pactCommands[0];
                _pactView.UpdateCurrentPact();

                return;
            }
            
            _pactView.gameObject.SetActive(false);
        }

        public void HandleEndTurn() => CurrentPlayer?.EndTurn();
        public void HandleUndoAction() => CurrentPlayer?.UndoAction();

        public void DisplayGameEndScreen()
        {
            _gameEndView.SetActive(true);
        }

        public void DisplayAttackedScreen(string attackerName)
        {
            _attackedScreenView.SetActive(true);
            _attackedScreenView.GetComponentInChildren<TextMeshProUGUI>().text = $"You have been attacked by {attackerName}, prepare for war!";
        }
    }
}
