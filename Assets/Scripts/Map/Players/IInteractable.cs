using Assets.Scripts.Map.UI;

namespace Assets.Scripts.Map.Players
{
    public interface IInteractable
    {
        public ushort Id { get; set; }
        public void HandleCountyInteraction(CountyInteractionInfo interactionInfo);
    }
}