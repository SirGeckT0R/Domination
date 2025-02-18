using System.Collections.Generic;

namespace Assets.Scripts.Map.AI.Contexts
{
    public class Context
    {
        public Brain brain; 
        readonly Dictionary<string, object> data = new();

        public Context(Brain brain) {
            this.brain = brain;
        }
        public T GetData<T>(string key) => data.TryGetValue(key, out var value) ? (T)value : default;
        public void SetData(string key, object value) => data[key] = value;
    }
}
