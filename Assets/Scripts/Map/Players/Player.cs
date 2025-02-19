using Assets.Scripts.Map.AI;
using Assets.Scripts.Map.Managers;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Map.Players
{
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public string Name { get; set; }

        public Brain Brain { get; set; }
        [field: SerializeField] public int Money { get; set; } = 10;
        [field: SerializeField] public int Warriors { get; set; } = 15;

        private TurnManager turnManager;

        [Inject]
        public void Construct(TurnManager turnManager)
        {
            this.turnManager = turnManager;
        }

        private void Awake()
        {
            Brain = GetComponent<Brain>();
        }

        //public void CreateEconomicCommand()
        //{
        //    var command = new EconomicCommand(this, 5);

        //    turnManager.AddCommand(command);
        //}

        //public void CreateRelationsCommand()
        //{
        //    var command = new RelationsCommand(this, 4);

        //    turnManager.AddCommand(command);
        //}

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