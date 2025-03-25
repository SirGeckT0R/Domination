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

        public override IEnumerator ProduceCommand(AI.Contexts.Context data)
        {
            var action = Brain.FindAndProduceTheBestAction(data);
            yield return new WaitForSeconds(1f);
            OnCommandAdded?.Invoke(action);
        }
    }
}