using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform AttackTarget { get; private set; }

    [SerializeField] private Material _idleStateMaterial;
    [SerializeField] private Material _attackStateMaterial;
    [SerializeField] private Material _followStateMaterial;
    public int unitDamage = 5;

    [SerializeField] private bool _isControlledByPlayer = false;

    public GameObject attackEffect;

    internal void Attack(Transform target)
    {
        AttackTarget = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_isControlledByPlayer && other.CompareTag("Enemy") && AttackTarget == null)
        {
            AttackTarget = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isControlledByPlayer && other.CompareTag("Enemy") && AttackTarget == null)
        {
            AttackTarget = other.transform;
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

    //public void SetIdleMaterial()
    //{
    //    GetComponent<Renderer>().material = _idleStateMaterial;
    //}

    //public void SetAttackMaterial()
    //{
    //    GetComponent<Renderer>().material = _attackStateMaterial;
    //}

    //public void SetFollowMaterial()
    //{
    //    GetComponent<Renderer>().material = _followStateMaterial;
    //}

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
