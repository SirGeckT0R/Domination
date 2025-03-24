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

namespace Assets.Scripts.Map.Managers
{
    public class TurnManager : MonoBehaviour
    {
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
        }

        private void Start()
        {
            _context = new Context();
            _context.CountyManager = _countyManager;
            CurrentPlayer = Players[CurrentPlayerIndex];

            //var pactEvent2 = new CreatePactEvent(Players[1], Players[0], 3);
            //_context.RelationEvents.Add(pactEvent2);
            //Players[0].PactCommands.Add(pactEvent2);


            //var pactEvent1 = new CreatePactEvent(Players[2], Players[0], 3);
            //_context.RelationEvents.Add(pactEvent1);
            //Players[0].PactCommands.Add(pactEvent1);

            StartTurn();
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
                _context.RelationEvents = _context.RelationEvents.Where(relEvent => !relEvent.UpdateDuration()).ToList();
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
                if(CurrentPlayer is HumanPlayer humanPlayer)
                {
                    _inputManager.gameObject.SetActive(true);
                    _uiManager.DisplayPlayerHUD(humanPlayer, GetOtherRemainingPlayers());
                }

                UpdateContext();

                //var pactEvent = new RelationEvent(Players[CurrentPlayerIndex],
                //    Players[2],
                //    RelationEventType.SentPact, 3);
                //_context.RelationEvents.Add(pactEvent);

                StartCoroutine(CurrentPlayer.StartTurn(_context));
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

        public Context AddCommand(Command command)
        {
            if (Commands.Count < MaxCommandsPerTurn)
            {
                Commands.Add(command);

                command.Execute();
                CreateRelationEvent(command);
                UpdateContext();

                return _context;
            }

            return null;
        }

        public void CreateRelationEvent(Command command)
        {
            switch (command)
            {
                case AttackWeakestAndWealthiestCommand warCommand:
                    var warEvent = new RelationEvent(warCommand.Player, warCommand.AttackTarget, RelationEventType.War, 3);
                    _context.RelationEvents.Add(warEvent);

                    break;
                case SendPactCommand pactCommand:
                    var pactEvent = new CreatePactEvent(pactCommand.player, pactCommand.pactTarget, 3);
                    pactCommand.pactTarget.PactCommands.Add(pactEvent);
                    _context.RelationEvents.Add(pactEvent);

                    break;
            }
        }

        public void AcceptPact(AcceptPactCommand command, RelationEvent relEvent)
        {
            _context.RelationEvents.Remove(relEvent);
            var acceptPact = new RelationEvent(command.player, command.pactTarget, RelationEventType.AcceptedPact, 3);
            _context.RelationEvents.Add(acceptPact);
        }

        public void DeclinePact(DeclinePactCommand command, RelationEvent relEvent)
        {
            _context.RelationEvents.Remove(relEvent);
            var declinePact = new RelationEvent(command.player, command.pactTarget, RelationEventType.DeniedPact, 3);
            _context.RelationEvents.Add(declinePact);
        }

        private void UpdateContext()
        {
            var remaining = GetOtherRemainingPlayers();

            _context.CurrentPlayer = CurrentPlayer;
            _context.SetData("Warriors", CurrentPlayer.Warriors);
            _context.SetData("Money", CurrentPlayer.Money);
            _context.OtherPlayers = remaining;
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
    }
}