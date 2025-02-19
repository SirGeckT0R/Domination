using Assets.Scripts.Map.Players;
using System.Collections.Generic;

namespace Assets.Scripts.Map.AI.Contexts
{
    public class Context
    {
        public Player CurrentPlayer { get; set; }
        public Player[] OtherPlayers { get; set; }
        public string AttackTarget { get; set; }

        readonly Dictionary<string, object> data = new();

        public T GetData<T>(string key) => data.TryGetValue(key, out var value) ? (T)value : default;
        public void SetData(string key, object value) => data[key] = value;
    }
}
