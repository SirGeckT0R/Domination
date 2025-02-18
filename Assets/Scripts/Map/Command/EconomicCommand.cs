using UnityEngine;

public class EconomicCommand : ICommand
{
    private int money;
    private Player player;
    public EconomicCommand(Player player, int money)
    {
        this.player = player;
        this.money = money;
    }

    public void Execute()
    {
        player.Money += money;
        player.Warriors -= money;
        Debug.Log($"Executing an economic action with parameters {money}");
    }

    public void Undo()
    {
        Debug.Log($"Undoing an economic action with parameters {money}");
    }
}

