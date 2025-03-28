using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.PlayerInput;
using Assets.Scripts.Map.Players;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Map.Managers
{
    public class TurnManager : MonoBehaviour
    {
        //public static TurnManager Instance { get; private set; }
        private DataHolder _dataHolder;

        [SerializeField] public List<Player> Players;

        private float maxAmountOfTurns = 20;
        private int _globalTurnCount = 0;
        private bool _hasTurnStarted = false;
        private bool _hasGlobalTurnStarted = false;

        [field: SerializeField] public int MaxCommandsPerTurn { get; private set; } = 2;
        [field: SerializeField] public int MoneyPerLevel { get; private set; } = 1;
        [field: SerializeField] public int WarriorsPerLevel { get; private set; } = 1;

        public int CurrentPlayerIndex { get; private set; }
        public Player CurrentPlayer { get; private set; }

        public List<Command> Commands { get; private set; } = new List<Command>();
        public List<RelationEvent> RelationEvents { get; private set; } = new List<RelationEvent>();

        private Context _context;

        private CountyManager _countyManager;
        private PlayerInputManager _inputManager;
        private UIManager _uiManager;

        [Zenject.Inject]
        public void Construct(CountyManager countyManager, PlayerInputManager inputManager, UIManager uiManager)
        {
            _countyManager = countyManager;
            _inputManager = inputManager;
            _uiManager = uiManager;
            _dataHolder = DataHolder.Instance;
        }

        //private void Awake()
        //{
        //    if (Instance != null && Instance != this)
        //    {
        //        Destroy(gameObject);
        //        _dataHolder = DataHolder.Instance;
        //        Initialize();
        //        Instance.StartTurn();

        //        return;
        //    }

        //    //Instance = this;
        //    _dataHolder = DataHolder.Instance;
        //    //DontDestroyOnLoad(gameObject);
        //}

        private void Start()
        {
            Initialize();
            StartTurn();
        }

        private void Initialize()
        {
            _globalTurnCount = _dataHolder.TurnManagerState.GlobalTurnCount;
            CurrentPlayerIndex = _dataHolder.TurnManagerState.CurrentPlayerIndex;
            _hasGlobalTurnStarted = _dataHolder.TurnManagerState.HasGlobalTurnStarted;
            _hasTurnStarted = false;
            Commands = _dataHolder.TurnManagerState.Commands;
            RelationEvents = _dataHolder.TurnManagerState.RelationEvents;
            _context = _dataHolder.TurnManagerState.Context ?? new Context();

            foreach (var player in Players)
            {
                player.OnCommandAdded.AddListener(AddCommand);
                player.OnCommandRemoved.AddListener(RemoveLastCommand);
                player.OnTurnEnded.AddListener(EndTurn);
            }

            CurrentPlayer = Players[CurrentPlayerIndex];
            _context.RelationEvents = RelationEvents;
            _context.CountyManager = _countyManager;

            if (_dataHolder.CurrentWarResult != null)
            {
                var realtionEvent = RelationEvents[RelationEvents.Count - 1];
                var warCommand = Commands[Commands.Count - 1] as AttackCommand;
                switch (_dataHolder.CurrentWarResult.Winner)
                {
                    case Battleground.Enums.BattleOpponent.Player when _dataHolder.CurrentWarInfo.BattleType == BattleType.Attack:
                        warCommand.Player.Warriors = _dataHolder.CurrentWarResult.RemainingPlayerWarriorsCount;
                        warCommand.AttackTarget.Warriors = _dataHolder.CurrentWarResult.RemainingEnemyWarriorsCount;

                        break;
                }
                _dataHolder.CurrentWarResult = null;
            }
            //Instance._globalTurnCount = _dataHolder.TurnManagerState.GlobalTurnCount;
            //Instance.CurrentPlayerIndex = _dataHolder.TurnManagerState.CurrentPlayerIndex;
            //Instance._hasGlobalTurnStarted = _dataHolder.TurnManagerState.HasGlobalTurnStarted;
            //Instance._hasTurnStarted = false;
            //Instance._context = _dataHolder.TurnManagerState.Context ?? new Context();

            //Instance.Players = Players;

            //foreach (var player in Instance.Players)
            //{
            //    player.OnCommandAdded.AddListener(AddCommand);
            //    player.OnCommandRemoved.AddListener(RemoveLastCommand);
            //    player.OnTurnEnded.AddListener(EndTurn);
            //}

            //Instance.CurrentPlayer = Instance.Players[CurrentPlayerIndex];
            //Instance._context.CountyManager = Instance._countyManager;
        }

        private void StartTurn()
        {
            if (_globalTurnCount >= maxAmountOfTurns)
            {
                foreach (var player in Players)
                {
                    Debug.Log(player.Name + " " + player.Warriors + " " + player.Money);
                }

                return;
            }

            if (!_hasGlobalTurnStarted)
            {
                RelationEvents = RelationEvents.Where(relEvent => !relEvent.UpdateDuration()).ToList();
                _hasGlobalTurnStarted = true;
            }


            if (!_hasTurnStarted)
            {
                var isLost = _countyManager.CountyOwners[CurrentPlayer.Id].Count <= 0;
                if (isLost)
                {
                    EndTurn();

                    return;
                }

                _hasTurnStarted = true;
                if (CurrentPlayer is HumanPlayer humanPlayer)
                {
                    _inputManager.gameObject.SetActive(true);
                    _uiManager.DisplayPlayerHUD(humanPlayer, GetOtherRemainingPlayers());
                }

                UpdateContext();

                StartCoroutine(CurrentPlayer.ProduceCommand(_context));
            }
        }

        public void EndTurn()
        {
            _inputManager.gameObject.SetActive(false);
            Commands.Clear();

            var currentPlayer = CurrentPlayer;
            var playerCounties = _countyManager.CountyOwners[currentPlayer.Id];

            UpdateStats(currentPlayer, playerCounties);
            Debug.Log(currentPlayer.Name + " " + currentPlayer.Warriors + " " + currentPlayer.Money + " " + playerCounties.ToCommaSeparatedString());

            _hasTurnStarted = false;
            if (CurrentPlayerIndex >= Players.Count - 1)
            {
                RemoveLostPlayers();

                _globalTurnCount++;
                CurrentPlayerIndex = 0;
                CurrentPlayer = Players[CurrentPlayerIndex];
                _hasGlobalTurnStarted = false;
            }
            else
            {
                CurrentPlayerIndex++;
                CurrentPlayer = Players[CurrentPlayerIndex];
            }

            StartTurn();
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
        }

        private void RemoveLostPlayers()
        {
            var playersWhoLostIds = _countyManager.PlayersWhoLost();
            if (playersWhoLostIds.Count > 0)
            {
                Players.RemoveAll(player => playersWhoLostIds.Contains(player.Id));
            }
        }

        public void AddCommand(Command command)
        {
            Debug.Log(FindObjectsByType<Player>(FindObjectsSortMode.None).Length);

            if (Commands.Count >= MaxCommandsPerTurn)
            {
                return;
            }

            Commands.Add(command);

            if(command is not AttackCommand)
            {

                command.Execute();
            }
            CreateRelationEvent(command);
            UpdateContext();
            if (CurrentPlayer is AIPlayer aiPlayer && Commands.Count == MaxCommandsPerTurn)
            {
                EndTurn();

                return;
            }

            StartCoroutine(CurrentPlayer.ProduceCommand(_context));
        }

        public void CreateRelationEvent(Command command)
        {
            switch (command)
            {
                case AttackCommand warCommand:
                    var warEvent = new RelationEvent(warCommand.Player.Id, warCommand.AttackTarget.Id, RelationEventType.War, 3);
                    RelationEvents.Add(warEvent);
                    HandleWar(warCommand);

                    break;
            }
        }

        private void UpdateContext()
        {
            var remaining = GetOtherRemainingPlayers();

            _context.CurrentPlayer = CurrentPlayer;
            _context.SetData("Warriors", CurrentPlayer.Warriors);
            _context.SetData("Money", CurrentPlayer.Money);
            _context.OtherPlayers = remaining;
            _context.RelationEvents = RelationEvents;
        }

        private List<Player> GetOtherRemainingPlayers()
        {
            return Players.Where(
                player => player != CurrentPlayer
                && _countyManager.CountyOwners[player.Id].Count > 0
                ).ToList();
        }

        public void RemoveLastCommand()
        {
            var last = Commands.Count - 1;
            if (Commands.Count < 1 || Commands[last] is IUndoable)
            {
                return;
            }

            Commands[last].Undo();
            Commands.RemoveAt(last);
        }

        public void HandleWar(AttackCommand warCommand)
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

            _dataHolder.TurnManagerState.Initialize(_globalTurnCount, _hasTurnStarted, _hasGlobalTurnStarted, CurrentPlayerIndex, Commands, RelationEvents, _context);

            SceneManager.LoadScene("Battleground");
        }
    }
}