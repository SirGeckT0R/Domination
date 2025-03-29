using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Commands;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Map/TurnManagerState")]
    public class TurnManagerState : ScriptableObject
    {
        [field: SerializeField] public int GlobalTurnCount { get; private set; }
        [field: SerializeField] public bool HasTurnStarted { get; private set; }
        [field: SerializeField] public bool HasGlobalTurnStarted { get; private set; }
        [field: SerializeField] public int CurrentPlayerIndex { get; private set; }
        [field: SerializeField] public List<Command> Commands { get; private set; }
        [field: SerializeField] public List<RelationEvent> RelationEvents { get; private set; } = new List<RelationEvent>();
        [field: SerializeField] public Context Context { get; private set; }

        public void Initialize(int globalTurnCount, bool hasTurnStarted, bool hasGlobalTurnStarted, int currentPlayerIndex, List<Command> commands, List<RelationEvent> relationEvents, Context context)
        {
            GlobalTurnCount = globalTurnCount;
            HasTurnStarted = hasTurnStarted;
            HasGlobalTurnStarted = hasGlobalTurnStarted;
            CurrentPlayerIndex = currentPlayerIndex;
            Commands = new List<Command>(commands);
            RelationEvents = new List<RelationEvent>(relationEvents);
            Context = context;
        }

        public void Initialize(TurnManagerState state)
        {
            GlobalTurnCount = state.GlobalTurnCount;
            HasTurnStarted = state.HasTurnStarted;
            HasGlobalTurnStarted = state.HasGlobalTurnStarted;
            CurrentPlayerIndex = state.CurrentPlayerIndex;
            Commands = new List<Command>(state.Commands);
            RelationEvents = new List<RelationEvent>(state.RelationEvents);
            Context = state.Context;
        }
    }
}
