using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [field: SerializeField] public string Name { get; set; }

    public int Money { get; set; } = 10;
    public int Warriors { get; set; } = 15;

    private TurnManager turnManager;

    [Inject]
    public void Construct(TurnManager turnManager)
    {
        this.turnManager = turnManager;
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

    public void StartTurn()
    {
        CreateEconomicCommand();
        CreateRelationsCommand();
        turnManager.EndTurn();
    }
}
