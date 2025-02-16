using Helpers;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public delegate void DetectionHandler(Unit unit, TriggerType triggerType);
    public event DetectionHandler OnDetected;

    [field: SerializeField] public LayerMask HostileMask { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        CheckHostile(other, TriggerType.Enter);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckHostile(other, TriggerType.Stay);
    }


    private void OnTriggerExit(Collider other)
    {
        CheckHostile(other, TriggerType.Exit);
    }

    private void CheckHostile(Collider other, TriggerType triggerType)
    {
        var isHostile = LayerMaskHelper.IsInLayerMask(other.gameObject, HostileMask);
        var unitComponent = other.transform.GetComponent<Unit>();

        if (isHostile && unitComponent != null)
        {
            OnDetected?.Invoke(unitComponent, triggerType);
        }
    }
}
