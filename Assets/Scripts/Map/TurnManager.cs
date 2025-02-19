using Assets.Scripts.Map.AI.Contexts;
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
                UpdateContext();

                return _context;
            }

            return null;
        }

        private void UpdateContext()
        {
            var remaining = Players.Where(player => player != Players[CurrentPlayerIndex]).ToArray();

            _context.CurrentPlayer = Players[CurrentPlayerIndex];
            _context.SetData("Warriors", Players[CurrentPlayerIndex].Warriors);
            _context.SetData("Money", Players[CurrentPlayerIndex].Money);
            _context.OtherPlayers = remaining;
        }

        public void RemoveLastCommand()
        {
            var last = Commands.Count - 1;
            Commands[last].Undo();

            Commands.RemoveAt(last);
        }
    }
}