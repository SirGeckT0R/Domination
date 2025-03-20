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
        //public AIPlayer AttackTarget { get; set; }
        public Player PactTarget { get; set; }
        public CreatePactEvent CurrentPact{ get; set; }

        readonly Dictionary<string, object> data = new();
        public List<RelationEvent> RelationEvents { get; set; } = new List<RelationEvent>();
        public CountyManager CountyManager { get;  set; }

        public T GetData<T>(string key) => data.TryGetValue(key, out var value) ? (T)value : default;
        public void SetData(string key, object value) => data[key] = value;
    }
}
