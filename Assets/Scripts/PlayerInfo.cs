using Assets.Scripts.Map.AI.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Map/PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [field: SerializeField] public ushort Id { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public int Money { get; set; }
        [field: SerializeField] public int Warriors { get; set; }
        public List<CreatePactEvent> PactCommands { get; private set; } = new List<CreatePactEvent>();

        public void Initialize(int money, int warriors, List<CreatePactEvent> commands)
        {
            Money = money;
            Warriors = warriors;
            PactCommands = commands;
        }

        public void Initialize(PlayerInfo info)
        {
            Name =  info.Name;
            Money = info.Money;
            Warriors = info.Warriors;
            PactCommands = info.PactCommands;
        }

        public void Deconstruct(out string name, out int money, out int warriors, out List<CreatePactEvent> commands)
        {
            name = Name;
            money = Money;
            warriors = Warriors;
            commands = PactCommands;
        }
    }
}
