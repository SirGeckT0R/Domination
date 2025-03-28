using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.Players
{
    public class HumanPlayer : Player
        //, IInteractable
    {
        //rework
        public List<Command> reactions;
        public List<RelationEventType> ignoreEvents;
        public bool IsHisTurn { get; private set; } = false;

        private Context _context;
        private CountyManager _countyManager;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        [Zenject.Inject]
        public void Construct(CountyManager countyManager)
        {
            _countyManager = countyManager;
        }

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
                OnTurnEnded?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("Removed economic command for human");
                OnCommandRemoved?.Invoke();
            }
        }

        public override IEnumerator ProduceCommand(Context data)
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

        public void SendPactToPlayer(Player player)
        {
            var command = ScriptableObject.CreateInstance<SendPactCommand>();
            command.UpdateContext(this, player, _context.RelationEvents);

            OnCommandAdded?.Invoke(command);
        }

        public void AcceptPact(CreatePactEvent pactEvent)
        {
            var acceptPact = reactions[0] as AcceptPactCommand;
            acceptPact.UpdateContext(pactEvent, _context.RelationEvents);
            acceptPact.Execute();

            PactCommands.Remove(pactEvent);
        }

        public void DeclinePact(CreatePactEvent pactEvent)
        {
            var declinePact = reactions[1] as DeclinePactCommand;
            declinePact.UpdateContext(pactEvent, _context.RelationEvents);
            declinePact.Execute();

            PactCommands.Remove(pactEvent);
        }

        private void AddEconomicUpgrade(County county)
        {
            var command = ScriptableObject.CreateInstance<EconomicUpgradeCommand>();
            command.UpdateContext(county, this);

            OnCommandAdded?.Invoke(command);
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

            OnCommandAdded?.Invoke(command);
        }

        private void AttackCounty(County county)
        {
            var attackTarget = turnManager.Players.FirstOrDefault(player => player.Id == county.BelongsTo);
            var wereInvolvedInRelations = _context.RelationEvents.Any(relEvent => relEvent.ArePlayersInvolved(Id, attackTarget.Id)
                && !ignoreEvents.Contains(relEvent.EventType));
            if (wereInvolvedInRelations)
            {
                Debug.Log("Can't declare war");
                return;
            }

            var command = ScriptableObject.CreateInstance<AttackCommand>();
            command.UpdateContext(county, this, _countyManager, attackTarget);


            OnCommandAdded?.Invoke(command);
        }
    }
}
