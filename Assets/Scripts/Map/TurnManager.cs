using Assets.Scripts.Map.AI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [field: SerializeField] public Player[] Players;
    private int _currentPlayer = 0;
    private bool _hasTurnStarted = false;

    private float maxAmountOfTurns = 50;
    private int _turnCount = 0;

    [field: SerializeField] public int MaxCommandsPerTurn { get; private set; } = 2;
    public int CurrentPlayer
    {
        get => _currentPlayer;
        private set => _currentPlayer = value % Players.Length;
    }
    public List<ICommand> Commands { get; private set; } = new List<ICommand>();

    private void StartTurn()
    {
        if (_turnCount >= maxAmountOfTurns)
        {
            return;
        }
        if (!_hasTurnStarted)
        {
            _hasTurnStarted = true;
            var remaining = Players.Where(player => player != Players[CurrentPlayer]).ToList();
            Players[CurrentPlayer].StartTurn(new ContextData(Players[CurrentPlayer], remaining));
        }
    }
    private void Start()
    {
       StartTurn();
    }

    public void EndTurn()
    {
        foreach (var command in Commands)
        {
            command.Execute();
        }

        Commands.Clear();

        _hasTurnStarted = false;
        _turnCount++;
        CurrentPlayer += 1;
        StartTurn();
    }


    public void AddCommand(ICommand command)
    {
        if (Commands.Count < MaxCommandsPerTurn)
        {
            Commands.Add(command);
        }
    }

    public void RemoveLastCommand()
    {
        var last = Commands.Count - 1;
        Commands[last].Undo();

        Commands.RemoveAt(last);
    }
}
