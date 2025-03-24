using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.UI;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.Players
{
    public class HumanPlayer : Player, IInteractable
    {
        public bool IsHisTurn { get; private set; } = false;

        private Context _context;
        private CountyManager _countyManager;

        [Zenject.Inject]
        public void Construct(CountyManager countyManager)
        {
            _countyManager = countyManager;
        }

        //private IEnumerator Start()
        //{
        //    yield return new WaitUntil(() => _countyManager.didStart);

        //    var playerCounties = _countyManager.CountyOwners[Id];
        //    foreach (var county in playerCounties)
        //    {
        //        county.OnCountyInteraction.AddListener(HandleCountyInteraction);
        //    }
        //}

        private void Update()
        {
            if (!IsHisTurn)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {

                Debug.Log("Ended turn for human");
                IsHisTurn = false;
                turnManager.EndTurn();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("Removed economic command for human");
                turnManager.RemoveLastCommand();
            }
        }

        public override IEnumerator StartTurn(Context data)
        {
            IsHisTurn = true;
            _context = data;

            yield break; 
        }

        public void HandleCountyInteraction(CountyInteractionInfo interactionInfo)
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
                    AttackCounty(interactionInfo.County);

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
        private void AttackCounty(County county)
        {
            var attackTarget = turnManager.Players.FirstOrDefault(player => player.Id == county.BelongsTo);
            var wereInvolvedInRelations = _context.RelationEvents.Any(relEvent => relEvent.ArePlayersInvolved(this, attackTarget));
            if (wereInvolvedInRelations)
            {
                Debug.Log("Can't declare war");
                return;
            }

            var command = ScriptableObject.CreateInstance<AttackWeakestAndWealthiestCommand>();
            command.UpdateContext(county, this, _countyManager, attackTarget);

            turnManager.AddCommand(command);
        }

        //public void AddCountyListener(County county)
        //{
        //    county.OnCountyInteraction.AddListener(HandleCountyInteraction);
        //}

        //public void RemoveCountyListener(County county)
        //{
        //    county.OnCountyInteraction.RemoveListener(HandleCountyInteraction);
        //}
    }
}
