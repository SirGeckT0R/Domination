using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.Managers
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] public Player[] Players;

        private float maxAmountOfTurns = 50;
        private int _turnCount = 0;
        private bool _hasTurnStarted = false;

        [field: SerializeField] public int MaxCommandsPerTurn { get; private set; } = 2;

        private int _currentPlayerIndex = 0;
        public int CurrentPlayerIndex
        {
            get => _currentPlayerIndex;
            private set => _currentPlayerIndex = value % Players.Length;
        }

        public List<Command> Commands { get; private set; } = new List<Command>();

        private Context _context;

        private void Start()
        {
            _context = new Context();


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
            if (_turnCount >= maxAmountOfTurns)
            {
                foreach (var player in Players)
                {
                    Debug.Log(player.Name + " " + player.Warriors + " " + player.Money);
                }

                return;
            }

            if (!_hasTurnStarted)
            {
                _hasTurnStarted = true;

                UpdateContext();
                
                //var pactEvent = new RelationEvent(Players[CurrentPlayerIndex],
                //    Players[2],
                //    RelationEventType.SentPact, 3);
                //_context.RelationEvents.Add(pactEvent);


                Players[CurrentPlayerIndex].StartTurn(_context);
            }
        }

        public void EndTurn()
        {
            Commands.Clear();

            Debug.Log(Players[CurrentPlayerIndex].Name + " " + Players[CurrentPlayerIndex].Warriors + " " + Players[CurrentPlayerIndex].Money);

            _hasTurnStarted = false;
            _turnCount++;
            CurrentPlayerIndex += 1;

            StartTurn();
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
            var warCommand = command as AttackWeakestAndWealthiestCommand;
            var pactCommand = command as SendPactCommand;

            //switch here
            if (warCommand != null)
            {
                var warEvent = new RelationEvent(warCommand.player, warCommand.attackTarget, RelationEventType.War,5);
                _context.RelationEvents.Add(warEvent);
            }

            if(pactCommand != null)
            {
                var pactEvent = new CreatePactEvent(pactCommand.player, pactCommand.pactTarget, 3);
                pactCommand.pactTarget.PactCommands.Add(pactEvent);
                _context.RelationEvents.Add(pactEvent);
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
            var remaining = Players.Where(player => player != Players[CurrentPlayerIndex]).ToArray();

            _context.CurrentPlayer = Players[CurrentPlayerIndex];
            _context.SetData("Warriors", Players[CurrentPlayerIndex].Warriors);
            _context.SetData("Money", Players[CurrentPlayerIndex].Money);
            _context.OtherPlayers = remaining;

            if (!_hasTurnStarted)
            {
                _context.RelationEvents = _context.RelationEvents.Where(relEvent => !relEvent.UpdateDuration()).ToList();
            }
        }

        public void RemoveLastCommand()
        {
            var last = Commands.Count - 1;
            Commands[last].Undo();

            Commands.RemoveAt(last);
        }
    }
}