using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Map.Players
{
    public abstract class Player : MonoBehaviour
    {
        [field: SerializeField] public ushort Id { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public int Money { get; set; } = 10;

        [SerializeField] private int warriors = 15;
        public int Warriors
        {
            get => warriors; 
            set
            {
                if (value < 0)
                {
                    warriors = 0;
                    return;
                }

                warriors = value;
            }
        }

        protected TurnManager turnManager;

        public List<CreatePactEvent> PactCommands { get; private set; } = new List<CreatePactEvent>();

        [Inject]
        public void Construct(TurnManager turnManager)
        {
            this.turnManager = turnManager;
        }

        public void AcceptPact(AcceptPactCommand command, CreatePactEvent pactEvent)
        {
            turnManager.AcceptPact(command, pactEvent);
        }

        public void DeclinePact(DeclinePactCommand command, CreatePactEvent pactEvent)
        {
            turnManager.DeclinePact(command, pactEvent);
        }

        public void UndoLastAction()
        {
            turnManager.RemoveLastCommand();
        }

        public abstract IEnumerator StartTurn(AI.Contexts.Context data);
    }
}
