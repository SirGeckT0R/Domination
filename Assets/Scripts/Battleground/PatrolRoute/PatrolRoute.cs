using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    [field: SerializeField] public List<Transform> Nodes { get; private set; }
}
