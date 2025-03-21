using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Map.Players
{
    public class HumanPlayer : Player
    {
        [field: SerializeField] public Command EconomicCommand { get; private set; }

        private bool _isHisTurn = false;
        private Context context;

        private CountyManager _countyManager;

        [Zenject.Inject]
        public void Construct(CountyManager countyManager)
        {
            _countyManager = countyManager;
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => _countyManager.didStart);

            var playerCounties = _countyManager.CountyOwners[Id];
            foreach (var county in playerCounties)
            {
                county.OnCountyInteractionEvent.AddListener((interactionInfo) => HandleCountyInteraction(interactionInfo));
            }
        }

        private void Update()
        {
            if (!_isHisTurn)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                EconomicCommand.UpdateContext(context);
                Debug.Log("Added economic command for human");
                context = turnManager.AddCommand(EconomicCommand);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {

                Debug.Log("Ended turn for human");
                _isHisTurn = false;
                turnManager.EndTurn();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("Removed economic command for human");
                turnManager.RemoveLastCommand();
            }
        }

        public override void StartTurn(Context data)
        {
            context = data;
            _isHisTurn = true;
        }

        private void HandleCountyInteraction(CountyInteractionInfo interactionInfo)
        {
            switch (interactionInfo)
            {
                case { InteractionType: CountyInteractionType.EconomicUpgrade }:
                    Debug.Log("Upgrade economic level" + interactionInfo);
                    AddEconomicUpgrade(interactionInfo.County);

                    break;
                case { InteractionType: CountyInteractionType.MilitaryUpgrade }:
                    Debug.Log("Upgrade military level" + interactionInfo);
                    AddMilitaryUpgrade(interactionInfo.County);

                    break;
                case { InteractionType: CountyInteractionType.Attack }:
                    Debug.Log("Attacking enemy county" + interactionInfo);
                    break;
            }
        }

        private void AddEconomicUpgrade(County county)
        {
            var command = ScriptableObject.CreateInstance<EconomicUpgradeCommand>();
            command.UpdateContext(county, this);

            turnManager.AddCommand(command);
        }

        private void AddMilitaryUpgrade(County county)
        {
            if(Money < _countyManager.PriceForMilitaryUpgrade)
            {
                Debug.Log("Insufficient amount of money for military upgrade");
                return;
            }

            var command = ScriptableObject.CreateInstance<MilitaryUpgradeCommand>();
            command.UpdateContext(county, this);

            turnManager.AddCommand(command);
        }
    }
}
