using Assets.Scripts.Map.AI;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [field: SerializeField] public string Name { get; set; }

    public Brain Brain { get; set; }
    public int Money { get; set; } = 10;
    public int Warriors { get; set; } = 15;

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

    public void CreateEconomicCommand()
    {
        var command = new EconomicCommand(this, 5);

        turnManager.AddCommand(command);
    }

    public void CreateRelationsCommand()
    {
        var command = new RelationsCommand(this, 4);

        turnManager.AddCommand(command);
    }

    public void UndoLastAction()
    {
        turnManager.RemoveLastCommand();
    }

    public void StartTurn(ContextData data)
    {
        Brain.FindAndProduceTheBestAction(data);

        //CreateEconomicCommand();
        //CreateRelationsCommand();
        turnManager.EndTurn();
    }
}
