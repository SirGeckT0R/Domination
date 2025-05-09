using Assets.Scripts.Battleground.Enums;
using Assets.Scripts.MainMenu.UI;
using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.PlayerInput;
using Assets.Scripts.Map.Players;
using Assets.Scripts.Map.UI.GameLog;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Map.Managers
{
    public class TurnManager : MonoBehaviour
    {
        [field: SerializeField] public int MaxAmountOfTurns { get; private set; } = 20;
        [field: SerializeField] public int MaxCommandsPerTurn { get; private set; } = 2;
        [field: SerializeField] public int MoneyPerLevel { get; private set; } = 1;
        [field: SerializeField] public int WarriorsPerLevel { get; private set; } = 1;

        [field:SerializeField] public List<Player> Players { get; private set; }
        [field: SerializeField] public AudioClip _declareWarClip;

        private int _currentPlayerIndex;
        private Player _currentPlayer;
        private List<Command> _commands= new List<Command>();
        private List<RelationEvent> _relationEvents = new List<RelationEvent>();

        private int _globalTurnCount = 0;
        private bool _hasTurnStarted = false;
        private bool _hasGlobalTurnStarted = false;
        private DataHolder _dataHolder;
        private Context _context;

        private CountyManager _countyManager;
        private PlayerInputManager _inputManager;
        private UIManager _uiManager;
        [SerializeField] private LoadingScreen _loadingScreen;

        [Zenject.Inject]
        public void Construct(CountyManager countyManager, PlayerInputManager inputManager, UIManager uiManager)
        {
            _countyManager = countyManager;
            _inputManager = inputManager;
            _uiManager = uiManager;

            _dataHolder = DataHolder.Instance;
        }

        private void Start()
        {
            Initialize();
            StartTurn();
        }

        private void Initialize()
        {
            SetupFromDataHolder();

            if (_dataHolder.CurrentWarResult != null)
            {
                HandleWarResult();
            }

            if (_commands.Count == MaxCommandsPerTurn)
            {
                EndTurn();
            }
        }

        private void StartTurn()
        {
            if (_globalTurnCount >= MaxAmountOfTurns)
            {
                _uiManager.DisplayGameEndScreen(Players);

                return;
            }

            if (!_hasGlobalTurnStarted)
            {
                _relationEvents = _relationEvents.Where(relEvent => !relEvent.UpdateDuration()).ToList();
                _hasGlobalTurnStarted = true;
            }

            if (!_hasTurnStarted)
            {
                var isLost = _countyManager.CountyOwners[_currentPlayer.Id].Count <= 0;
                if (isLost)
                {
                    EndTurn();

                    return;
                }

                _hasTurnStarted = true;
                if (_currentPlayer is HumanPlayer humanPlayer)
                {
                    _inputManager.gameObject.SetActive(true);
                    _uiManager.DisplayPlayerHUD(humanPlayer, GetOtherRemainingPlayers());
                }

                UpdateContext();

                StartCoroutine(_currentPlayer.ProduceCommand(_context));
            }
        }

        public void EndTurn()
        {
            _inputManager.gameObject.SetActive(false);
            _commands.Clear();

            var playerCounties = _countyManager.CountyOwners[_currentPlayer.Id];

            UpdateStats(_currentPlayer, playerCounties);
            var message = new MessageDto
            {
                Player = _currentPlayer.Name,
                Message = $"«авершил ход с {_currentPlayer.Warriors} воинами, {_currentPlayer.Money} единицами денег"
            };

            _uiManager.AddLogMessage(message);

            _hasTurnStarted = false;
            if (_currentPlayerIndex >= Players.Count - 1)
            {
                RemoveLostPlayers();

                _globalTurnCount++;
                _currentPlayerIndex = 0;
                _hasGlobalTurnStarted = false;
            }
            else
            {
                _currentPlayerIndex++;
            }

            _currentPlayer = Players[_currentPlayerIndex];

            StartTurn();
        }

        public void AddCommand(Command command)
        {
            if (_commands.Count >= MaxCommandsPerTurn)
            {
                return;
            }

            _commands.Add(command);

            if (command is AttackCommand warCommand)
            {
                var isLoading = HandleWarCommand(warCommand);

                if (isLoading)
                {
                    return;
                }
            }

            var message = command.Execute();
            _uiManager.AddLogMessage(message);
            UpdateContext();

            if (_currentPlayer is AIPlayer aiPlayer && _commands.Count == MaxCommandsPerTurn)
            {
                EndTurn();

                return;
            }

            StartCoroutine(_currentPlayer.ProduceCommand(_context));
        }

        private bool HandleWarCommand(AttackCommand command)
        {
            var warEvent = new RelationEvent(command.Player.Id, command.AttackTarget.Id, RelationEventType.War, 3);
            _relationEvents.Add(warEvent);
            if (command.Player is HumanPlayer || command.AttackTarget is HumanPlayer)
            {
                StartWar(command);

                return true;
            }

            return false;
        }

        private void UpdateStats(Player currentPlayer, List<County> playerCounties)
        {
            var totalEconomicLevel = 0;
            var totalMilitaryLevel = 0;
            foreach (var county in playerCounties)
            {
                totalEconomicLevel += county.EconomicLevel;
                totalMilitaryLevel += county.MilitaryLevel;
            }

            currentPlayer.Money += MoneyPerLevel * totalEconomicLevel;
            currentPlayer.Warriors += WarriorsPerLevel * totalMilitaryLevel;

            if(currentPlayer is HumanPlayer humanPlayer)
            {
                _uiManager.UpdatePlayerStats(humanPlayer);
            }
        }

        private void RemoveLostPlayers()
        {
            var playersWhoLostIds = _countyManager.PlayersWhoLost();
            if (playersWhoLostIds.Count > 0)
            {
                Players.RemoveAll(player => playersWhoLostIds.Contains(player.Id));
            }
        }

        private void UpdateContext()
        {
            var remaining = GetOtherRemainingPlayers();

            _context.CurrentPlayer = _currentPlayer;
            _context.OtherPlayers = remaining;
            _context.RelationEvents = _relationEvents;
        }

        private List<Player> GetOtherRemainingPlayers()
        {
            return Players.Where(
                player => player != _currentPlayer
                && _countyManager.CountyOwners[player.Id].Count > 0
                ).ToList();
        }

        public void RemoveLastCommand()
        {
            var last = _commands.Count - 1;
            if (_commands.Count < 1 || _commands[last] is IUndoable)
            {
                return;
            }

            _commands[last].Undo();
            _commands.RemoveAt(last);
        }

        public void StartWar(AttackCommand warCommand)
        {
            WarInfo warInfo = ScriptableObject.CreateInstance<WarInfo>();
            switch (warCommand.Player)
            {
                case AIPlayer:
                    warInfo.Initialize(
                        BattleType.Defend,
                        playerWarriorsCount: warCommand.AttackTarget.Warriors,
                        enemyWarriorsCount: warCommand.Player.Warriors
                    );

                    _uiManager.DisplayAttackedScreen(warCommand.Player.Name);

                    break;
                case HumanPlayer:
                    warInfo.Initialize(
                        BattleType.Attack,
                        playerWarriorsCount: warCommand.Player.Warriors,
                        enemyWarriorsCount: warCommand.AttackTarget.Warriors
                    );

                    break;
            }

            _dataHolder.CurrentWarInfo = warInfo;

            _dataHolder.TurnManagerState.Initialize(_globalTurnCount, _hasTurnStarted, _hasGlobalTurnStarted, _currentPlayerIndex, _commands, _relationEvents, _context);
            StartCoroutine(LoadBattle());
        }

        private IEnumerator LoadBattle()
        {
            SoundManager.Instance.PlaySound(_declareWarClip);
            yield return new WaitForSeconds(5f);

            _loadingScreen.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void HandleWarResult()
        {
            var relationEvent = _relationEvents[_relationEvents.Count - 1];
            var warCommand = _commands[_commands.Count - 1] as AttackCommand;
            var sender = Players.FirstOrDefault(player => player.Id == warCommand.PlayerId);
            var receiver = Players.FirstOrDefault(player => player.Id == warCommand.AttackTargetId);
            var county = _countyManager.AllCounties.FirstOrDefault(county => county.Id == warCommand.CountyId);

            warCommand.BattleType = _dataHolder.CurrentWarInfo.BattleType;
            warCommand.HasWon = _dataHolder.CurrentWarResult.Winner == BattleOpponent.Player;
            warCommand.UpdateContext(county, sender, _countyManager, receiver, _dataHolder.CurrentWarResult);

            var message = warCommand.Execute();

            _uiManager.AddLogMessage(message);
            _dataHolder.CurrentWarResult = null;
        }

        private void SetupFromDataHolder()
        {
            _globalTurnCount = _dataHolder.TurnManagerState.GlobalTurnCount;
            _currentPlayerIndex = _dataHolder.TurnManagerState.CurrentPlayerIndex;
            _hasGlobalTurnStarted = _dataHolder.TurnManagerState.HasGlobalTurnStarted;
            _hasTurnStarted = false;
            _commands = _dataHolder.TurnManagerState.Commands;
            _relationEvents = _dataHolder.TurnManagerState.RelationEvents;
            _context = _dataHolder.TurnManagerState.Context ?? new Context();
            _currentPlayer = Players[_currentPlayerIndex];

            foreach (var player in Players)
            {
                player.OnCommandAdded.AddListener(AddCommand);
                player.OnCommandRemoved.AddListener(RemoveLastCommand);
                player.OnTurnEnded.AddListener(EndTurn);
            }

            _context.RelationEvents = _relationEvents;
            _context.CountyManager = _countyManager;
        }
    }
}