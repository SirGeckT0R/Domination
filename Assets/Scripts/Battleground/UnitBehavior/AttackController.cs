using Helpers;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [field: Header("Target")]
    [field: SerializeField] public Unit Target { get; private set; }

    [field: Header("Settings")]
    [field: SerializeField] public int UnitDamage { get; private set; } = 5;
    [field: SerializeField] public float AttackingDistance { get; private set; } = 1.5f;
    [field: SerializeField] public float StopAttackingDistance { get; private set; } = 3f;
    [field: SerializeField] public float AttacksPerSecond { get; private set; } = 2f;
    [field: SerializeField] public bool IsControlledByPlayer { get; private set; } = false;
    [field: SerializeField] public LayerMask HostileMask { get; private set; }

    [field: Header("Effects")]
    [field: SerializeField] public GameObject VisualEffect { get; private set; }

    public void SetAttackTarget(Unit target)
    {
        Target = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        var isHostile = LayerMaskHelper.IsInLayerMask(other.gameObject, HostileMask);
        var unitComponent = other.transform.GetComponent<Unit>();

        if (isHostile && unitComponent != null)
        {
            SetAttackTarget(unitComponent);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var isHostile = LayerMaskHelper.IsInLayerMask(other.gameObject, HostileMask);
        var unitComponent = other.transform.GetComponent<Unit>();

        if (isHostile && Target == null && unitComponent != null)
        {
            SetAttackTarget(unitComponent);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        var isHostile = LayerMaskHelper.IsInLayerMask(other.gameObject, HostileMask);

        if (isHostile && Target != null)
        {
            SetAttackTarget(null);
        }
    }

    public void PlayEffects()
    {
        VisualEffect.SetActive(true);
    }

    public void StopEffects()
    {
        VisualEffect.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 2.4f);
    }
}
