using Assets.Scripts.Map.AI;

namespace Assets.Scripts.Map.Players
{
    public class AIPlayer : Player
    {
        public Brain Brain { get; set; }

        private void Awake()
        {
            Brain = GetComponent<Brain>();
        }
        //public void LoseCounty(Guid countyId)
        //{
        //    Counties.Remove(countyId);
        //}

        //public void AcquireCounty(Guid countyId)
        //{
        //    Counties.Add(countyId);
        //}
        public override void StartTurn(AI.Contexts.Context data)
        {
            var action = Brain.FindAndProduceTheBestAction(data);
            data = turnManager.AddCommand(action);

            action = Brain.FindAndProduceTheBestAction(data);
            turnManager.AddCommand(action);

            turnManager.EndTurn();
        }
    }
}