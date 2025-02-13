using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Unit AttackTarget { get; private set; }

    [field: SerializeField] public int UnitDamage { get; private set; } = 5;
    [field: SerializeField] public float AttackingDistance { get; private set; } = 1.5f;
    [field: SerializeField]  public float StopAttackingDistance { get; private set; } = 3f;
    [field: SerializeField] public float AttacksPerSecond { get; private set; } = 2f;

    [field: SerializeField] public bool IsControlledByPlayer { get; private set; } = false;

    public GameObject attackEffect;

    internal void Attack(Unit target)
    {
        AttackTarget = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        var unitComponent = other.transform.GetComponent<Unit>();

        if (IsControlledByPlayer && other.CompareTag("Enemy") && AttackTarget == null && unitComponent != null)
        {
            AttackTarget = unitComponent;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var unitComponent = other.transform.GetComponent<Unit>();

        if (IsControlledByPlayer && other.CompareTag("Enemy") && AttackTarget == null && unitComponent != null)
        {
            AttackTarget = unitComponent;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && AttackTarget != null)
        {
            AttackTarget = null;
        }
    }

    public void PlayEffects()
    {
        attackEffect.SetActive(true);
    }

    public void StopEffects()
    {
        attackEffect.SetActive(false);
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
