using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Players;
using UnityEngine;
namespace Assets.Scripts.Map.Commands
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/EconomicCommand")]
    public class EconomicCommand : Command
    {
        public int money;
        private Player player;

        public override void UpdateContext(Context context)
        {
            this.player = context.CurrentPlayer;
        }

        //public void Execute()
        //{
        //    player.Money += money;
        //    player.Warriors -= money;
        //    Debug.Log($"Executing an economic action with parameters {money}");
        //}

        public override void Execute()
        {
            player.Money += money;
            player.Warriors -= 1;
            Debug.Log($"Executing an economic action with parameters {money}");
        }

        //public void Undo()
        //{
        //    Debug.Log($"Undoing an economic action with parameters {money}");
        //}

        public override void Undo()
        {
            player.Money -= money;
            player.Warriors += 1;
            Debug.Log($"Undoing an economic action with parameters {money}");
        }
    }
}
