using Assets.Scripts.Map.AI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Map.Players
{
    public class AIPlayer : Player
    {
        public Brain Brain { get; set; }

        protected override void Awake()
        {
            base.Awake();
            Brain = GetComponent<Brain>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        public override IEnumerator ProduceCommand(AI.Contexts.Context data)
        {
            var action = Brain.FindAndProduceTheBestAction(data);
            yield return new WaitForSeconds(1f);
            OnCommandAdded?.Invoke(action);
        }
    }
}