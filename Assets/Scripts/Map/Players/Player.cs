using Assets.Scripts.Map.AI;
using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Map.Players
{
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public List<Guid> Counties { get; set; }
        public Brain Brain { get; set; }
        [field: SerializeField] public int Money { get; set; } = 10;
        [field: SerializeField] public int Warriors { get; set; } = 15;

        private TurnManager turnManager;

        public List<CreatePactEvent> PactCommands { get; private set; } = new List<CreatePactEvent>();

        [Inject]
        public void Construct(TurnManager turnManager)
        {
            this.turnManager = turnManager;
        }

        private void Awake()
        {
            Brain = GetComponent<Brain>();
        }

        public void AcceptPact(AcceptPactCommand command, CreatePactEvent pactEvent)
        {
            turnManager.AcceptPact(command, pactEvent);
        }

        public void DeclinePact(DeclinePactCommand command, CreatePactEvent pactEvent)
        {
            turnManager.DeclinePact(command, pactEvent);
        }

        public void LoseCounty(Guid countyId)
        {
            Counties.Remove(countyId);
        }

        public void AcquireCounty(Guid countyId)
        {
            Counties.Add(countyId);
        }

        public void UndoLastAction()
        {
            turnManager.RemoveLastCommand();
        }

        public void StartTurn(AI.Contexts.Context data)
        {
            var action = Brain.FindAndProduceTheBestAction(data);
            data = turnManager.AddCommand(action);

            action = Brain.FindAndProduceTheBestAction(data);
            turnManager.AddCommand(action);

            turnManager.EndTurn();
        }
    }
}