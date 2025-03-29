using UnityEngine;
using Assets.Scripts.DataStates;
using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class DataHolder : MonoBehaviour
    {
        public static DataHolder Instance { get; private set; }
        [field: SerializeField] public TurnManagerState OriginalTurnManagerState { get; set; }

        [field: SerializeField] public TurnManagerState TurnManagerState { get; set; } = null;
        [field: SerializeField] public WarInfo CurrentWarInfo { get; set; } = null;
        [field: SerializeField] public WarResult CurrentWarResult { get; set; } = null;

        [SerializeField] public SerializableDictionary<ushort, PlayerInfo> InspectorPlayerInfos;
        public Dictionary<ushort, PlayerInfo> OriginalPlayerInfos { get; set; }
        public Dictionary<ushort, PlayerInfo> PlayerInfos { get; set; } = new();

        [SerializeField] public SerializableDictionary<ushort, CountyInfo> InspectorCountyInfos;
        public Dictionary<ushort, CountyInfo> OriginalCountyInfos { get; set; }
        public Dictionary<ushort, CountyInfo> CountyInfos { get; set; } = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);

                return;
            }

            OriginalPlayerInfos = InspectorPlayerInfos.ToDictionary();
            OriginalCountyInfos = InspectorCountyInfos.ToDictionary();

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        private void Initialize()
        {
            TurnManagerState = ScriptableObject.CreateInstance<TurnManagerState>();
            TurnManagerState.Initialize(OriginalTurnManagerState);

            foreach (var player in OriginalPlayerInfos)
            {
                var playerInfo = ScriptableObject.CreateInstance<PlayerInfo>();
                playerInfo.Initialize(player.Value);
                PlayerInfos.Add(player.Key, playerInfo);
            }

            foreach (var county in OriginalCountyInfos)
            {
                var countyInfo = ScriptableObject.CreateInstance<CountyInfo>();
                countyInfo.Initialize(county.Value);
                CountyInfos.Add(county.Key, countyInfo);
            }
        }
    }
}

[Serializable]
public class SerializableDictionary<TKey, TValue>
{
    [SerializeField]
    public DictionaryItem<TKey, TValue>[] items;
    public Dictionary<TKey, TValue> ToDictionary()
    {
        var dict = new Dictionary<TKey, TValue>();
        foreach (var item in items)
        {
            dict.Add(item.Key, item.Value);
        }

        return dict;
    }
}

[Serializable]
public class DictionaryItem<TKey, TValue>
{
    [SerializeField]
    public TKey Key;

    [SerializeField]
    public TValue Value;
}