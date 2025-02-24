using System;
using UnityEngine;

namespace Assets.Scripts.Map.Counties
{
    public class County : MonoBehaviour
    {
        public Guid Id { get; set; } = new Guid();
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public int EconomicLevel { get; set; }
        [field: SerializeField] public int WarfareLevel { get; set; }
    }
}
