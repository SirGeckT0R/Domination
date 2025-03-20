using Assets.Scripts.Map.AI.Considerations;
using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.Managers;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Map.AI
{
    public class Brain : MonoBehaviour
    {
        public List<Command> actions;
        public CompositePactConsideration pactConsideration;
        public List<Command> reactions;

        private CountyManager _countyManager;

        [Inject]
        public void Construct(CountyManager countyManager)
        {
            _countyManager = countyManager;
        }

        void Awake()
        {
            foreach (var action in actions)
            {
                //action.Initialize(Context);
            }
        }

        public Command FindAndProduceTheBestAction(Contexts.Context context)
        {
            ConsiderPacts(context);

            Command bestAction = null;
            float highestUtility = float.MinValue;

            float utility = 0;
            foreach (var action in actions)
            {
                utility = action.CalculateUtility(context);
                if (utility > highestUtility)
                {
                    highestUtility = utility;
                    bestAction = action;
                }
            }

            bestAction?.UpdateContext(context);

            Debug.Log("Best action: " + bestAction.GetType().Name + " with utility: " + highestUtility);

            return bestAction;
        }

        private void ConsiderPacts(Contexts.Context context)
        {
            List<CreatePactEvent> pacts = context.CurrentPlayer.PactCommands;
            float utility = 0;
            foreach (var createPact in pacts)
            {
                context.CurrentPact = createPact;
                utility = pactConsideration.Evaluate(context);

                if (utility > 0.5)
                {
                    (reactions[0] as AcceptPactCommand).player = context.CurrentPlayer;
                    (reactions[0] as AcceptPactCommand).pactTarget = createPact.Sender;
                    reactions[0].Execute();
                    context.CurrentPlayer.AcceptPact(reactions[0] as AcceptPactCommand, createPact);
                }
                else
                {
                    (reactions[1] as DeclinePactCommand).player = context.CurrentPlayer;
                    (reactions[1] as DeclinePactCommand).pactTarget = createPact.Sender;
                    reactions[1].Execute();
                    context.CurrentPlayer.DeclinePact(reactions[1] as DeclinePactCommand, createPact);
                }

                context.WarTargetInfo = null;
                context.PactTarget = null;
            }

            pacts.Clear();
        }
    }
}
