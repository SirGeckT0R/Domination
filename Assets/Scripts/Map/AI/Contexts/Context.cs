using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.Players;
using System.Collections.Generic;

namespace Assets.Scripts.Map.AI.Contexts
{
    public class Context
    {
        public Player CurrentPlayer { get; set; }
        public List<Player> OtherPlayers { get; set; }
        public WarTargetInfo WarTargetInfo { get; set; }
        public Player PactTarget { get; set; }
        public CreatePactEvent CurrentPact { get; set; }
        public List<RelationEvent> RelationEvents { get; set; }
        public CountyManager CountyManager { get;  set; }
    }
}
