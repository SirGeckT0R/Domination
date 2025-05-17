using Assets.Scripts.Map.AI.Considerations;
using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.UI.GameLog;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map.AI
{
    public class Brain : MonoBehaviour
    {
        [field: SerializeField] public List<Command> Actions { get; private set; }
        [field: SerializeField] public List<Command> Reactions { get; private set; }
        [field: SerializeField] public CompositePactConsideration PactConsideration { get; private set; }
        [field: SerializeField] public Context Context { get; private set; }

        private UIManager _uiManager;

        [Zenject.Inject]
        public void Construct(UIManager uIManager)
        {
            _uiManager = uIManager;
        }

        public Command FindAndProduceTheBestAction(Context context)
        {
            Context = context;
            //better make it so AIPlayer calls ConsiderPacts itself, beacuse in current way it considers pacts twice during the turn
            ConsiderPacts();

            Command bestAction = null;
            float highestUtility = float.MinValue;

            float utility = 0;
            foreach (var action in Actions)
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

        private void ConsiderPacts()
        {
            List<CreatePactEvent> pacts = Context.CurrentPlayer.PactCommands;
            float utility = 0;
            foreach (var createPact in pacts)
            {
                Context.CurrentPact = createPact;
                utility = PactConsideration.Evaluate(Context);
                MessageDto resultMessage;

                if (utility > 0.5)
                {
                    //Just create via scriptable object
                    var acceptPact = Reactions[0] as AcceptPactCommand;
                    acceptPact.UpdateContext(Context.CurrentPlayer.Name, createPact, Context.RelationEvents);
                    resultMessage = acceptPact.Execute();
                }
                else
                {
                    var declinePact = Reactions[1] as DeclinePactCommand;
                    declinePact.UpdateContext(Context.CurrentPlayer.Name, createPact, Context.RelationEvents);
                    resultMessage = declinePact.Execute();
                }

                _uiManager.AddLogMessage(resultMessage);

                Context.WarTargetInfo = null;
                Context.PactTarget = null;
            }
            pacts.Clear();
        }
    }
}
