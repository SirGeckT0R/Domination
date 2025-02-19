using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Players;
using UnityEngine;

namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/RelationsCommand")]
    public class RelationsCommand : Command
    {
        public int losses;
        private Player player;

        public override void UpdateContext(Context context)
        {
            this.player = context.CurrentPlayer;
        }

        public override void Execute()
        {
            player.Warriors += losses;
            player.Money -= 1;
            Debug.Log($"Executing relations action with parameters {losses}");
        }

        //public void Undo()
        //{
        //    Debug.Log($"Undoing relations action with parameters {losses}");
        //}

        public override void Undo()
        {
            player.Warriors -= losses;
            player.Money += 1;
            Debug.Log($"Undoing relations action with parameters {losses}");
        }
    }
}