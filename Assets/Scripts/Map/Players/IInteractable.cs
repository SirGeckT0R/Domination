using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.UI;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Assets.Scripts.Map.Players
{
    public interface IInteractable
    {
        public ushort Id { get; set; }
        //remove 
        public List<CreatePactEvent> PactCommands { get; set; }
        //public UnityEvent OnPactResolved { get; }
        public void HandleCountyInteraction(CountyInteractionInfo interactionInfo);
        public void SendPactToPlayer(Player player);
        //Add create pact
    }
}