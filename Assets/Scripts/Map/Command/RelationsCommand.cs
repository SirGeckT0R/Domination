using UnityEngine;

public class RelationsCommand : ICommand
{
    public int losses;
    private Player player;

    public RelationsCommand(Player player,int losses)
    {
        this.player = player;
        this.losses = losses;
    }

    public void Execute()
    {
        player.Warriors += losses;
        player.Money -= losses;
        Debug.Log($"Executing relations action with parameters {losses}");
    }

    public void Undo()
    {
        Debug.Log($"Undoing relations action with parameters {losses}");
    }
}
