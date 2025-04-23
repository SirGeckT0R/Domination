using Assets.Scripts.Map.AI.Contexts;
using Assets.Scripts.Map.AI.Enums;
using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Commands;
using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.PlayerInput;
using Assets.Scripts.Map.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map.Players
{
    public class HumanPlayer : Player
    {
        public bool IsHisTurn { get; private set; } = false;
        [field: SerializeField] private List<RelationEventType> _ignoreEvents;

        private Context _context;
        private CountyManager _countyManager;
        private PlayerAudioComponent _playerAudioComponent;

        protected override void Awake()
        {
            base.Awake();
            _playerAudioComponent = GetComponent<PlayerAudioComponent>();
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
            var acceptPact = ScriptableObject.CreateInstance<AcceptPactCommand>();
            acceptPact.UpdateContext(Name, pactEvent, _context.RelationEvents);
            acceptPact.Execute();

            PactCommands.Remove(pactEvent);
        }

        public void DeclinePact(CreatePactEvent pactEvent)
        {
            var declinePact = ScriptableObject.CreateInstance<DeclinePactCommand>();
            declinePact.UpdateContext(Name, pactEvent, _context.RelationEvents);
            declinePact.Execute();

            PactCommands.Remove(pactEvent);
        }

        private void AddEconomicUpgrade(County county)
        {
            _playerAudioComponent.PlayEconomicUpgradeSound();

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

            _playerAudioComponent.PlayMilitaryUpgradeSound();

            var command = ScriptableObject.CreateInstance<MilitaryUpgradeCommand>();
            command.UpdateContext(county, this);

            OnCommandAdded?.Invoke(command);
        }

        private void AttackCounty(County county)
        {
            var attackTarget = turnManager.Players.FirstOrDefault(player => player.Id == county.BelongsTo);
            var wereInvolvedInRelations = _context.RelationEvents.Any(relEvent => relEvent.ArePlayersInvolved(Id, attackTarget.Id)
                && !_ignoreEvents.Contains(relEvent.EventType));
            if (wereInvolvedInRelations)
            {
                Debug.Log("Can't declare war");
                return;
            }

            var command = ScriptableObject.CreateInstance<AttackCommand>();
            command.UpdateContext(county, this, _countyManager, attackTarget);

            OnCommandAdded?.Invoke(command);
        }

        public void UndoAction()
        {
            if (!IsHisTurn)
            {
                return;
            }

            Debug.Log("Removed economic command for human");
            OnCommandRemoved?.Invoke();
        }

        public void EndTurn()
        {
            if (!IsHisTurn)
            {
                return;
            }


            _playerAudioComponent.PlayEndTurnSound();

            Debug.Log("Ended turn for human");
            IsHisTurn = false;
            OnTurnEnded?.Invoke();
        }
    }
}
