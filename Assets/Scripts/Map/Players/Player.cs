using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Assets.Scripts.Map.Players
{
    public abstract class Player : MonoBehaviour
    {
        [field: SerializeField] public ushort Id { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public int Money { get; set; } = 10;
        [field: SerializeField] public Sprite Icon { get; set; }

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

        protected DataHolder _dataHolder;
        protected TurnManager turnManager;

        public UnityEvent<Command> OnCommandAdded;
        public UnityEvent OnCommandRemoved;
        public UnityEvent OnTurnEnded;

        public List<CreatePactEvent> PactCommands { get; private set; } = new List<CreatePactEvent>();

        protected virtual void Awake()
        {
            _dataHolder = DataHolder.Instance;
            (Name, Money, Warriors, PactCommands, Icon) = _dataHolder.PlayerInfos[Id];
        }

        protected virtual void OnDestroy()
        {
            _dataHolder.PlayerInfos[Id].Initialize(Money, Warriors, PactCommands);
        }

        [Inject]
        public void Construct(TurnManager turnManager)
        {
            this.turnManager = turnManager;
        }

        public abstract IEnumerator ProduceCommand(AI.Contexts.Context data);
    }
}
