using Assets.Scripts.DataStates;
using System;

namespace Assets.Scripts.Misc
{
    [Serializable]
    public class CountyInfoDictionary : SerializableDictionary<ushort, CountyInfo> { }
}
