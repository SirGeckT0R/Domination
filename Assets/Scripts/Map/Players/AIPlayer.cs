using Assets.Scripts.Map.AI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Map.Players
{
    public class AIPlayer : Player
    {
        public Brain Brain { get; set; }

        private void Awake()
        {
            Brain = GetComponent<Brain>();
        } 

        public override IEnumerator StartTurn(AI.Contexts.Context data)
        {
            var action = Brain.FindAndProduceTheBestAction(data);
            data = turnManager.AddCommand(action);

            yield return new WaitForSeconds(1f);

            action = Brain.FindAndProduceTheBestAction(data);

            yield return new WaitForSeconds(1f);

            turnManager.AddCommand(action);
            turnManager.EndTurn();
        }
    }
}