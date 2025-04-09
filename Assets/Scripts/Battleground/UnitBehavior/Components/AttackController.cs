using UnityEngine;

public class AttackController : MonoBehaviour
{
    [field: Header("Target")]
    [field: SerializeField] public Unit Target { get; private set; }
    private Health _attackTargetHealth;

    [field: Header("Settings")]
    [field: SerializeField] public int UnitDamage { get; private set; } = 5;
    [field: SerializeField] public float AttackingDistance { get; private set; } = 1.5f;
    [field: SerializeField] public float StopAttackingDistance { get; private set; } = 3f;
    [field: SerializeField] public float AttacksPerSecond { get; private set; } = 2f;
    [field: SerializeField] public bool IsControlledByPlayer { get; private set; } = false;
    [field: SerializeField] public LayerMask Attackable { get; private set; }

    [field: Header("Effects")]
    [field: SerializeField] public GameObject VisualEffect { get; private set; }

    private DetectionZone _detectionZone;

    private void Start()
    {
        _detectionZone = transform.Find("DetectionZone").GetComponent<DetectionZone>();

        _detectionZone.OnDetected += HandleDetection;
    }

    public void SetTarget(Unit target)
    {
        Target = target;

        if (Target)
        {
            _attackTargetHealth = Target.GetComponent<Health>();
        }
    }

    public void AttackTarget()
    {
        var damage = UnitDamage;

        if (_attackTargetHealth)
        {
            _attackTargetHealth.TakeDamage(damage);
        }

        SoundManager.Instance.PlayInfantryAttackSound();
    }

    public void HandleDetection(Unit unit, TriggerType triggerType)
    {
        switch (triggerType)
        {
            case TriggerType.Enter:
                var shouldSwitchTargets = Target == null || Target.IsDead || IsNewTargetCloser(unit.transform);
                var newTarget = shouldSwitchTargets ? unit : Target;

                SetTarget(!newTarget.IsDead ? newTarget : null);

                break;

            case TriggerType.Stay:
                shouldSwitchTargets = Target == null || Target.IsDead || IsNewTargetCloser(unit.transform);
                newTarget = shouldSwitchTargets ? unit : Target;

                SetTarget(!newTarget.IsDead ? newTarget : null);

                break;

            case TriggerType.Exit:
                if (Target == unit)
                {
                    SetTarget(null);
                }

                break;
        }
    }

    private bool IsNewTargetCloser(Transform unit)
    {
        return Vector3.Distance(transform.position, unit.position) < Vector3.Distance(transform.position, Target.transform.position);
    }

    public void PlayEffects()
    {
        if (VisualEffect != null)
        {
            VisualEffect.SetActive(true);
        }
    }

    public void StopEffects()
    {
        if (VisualEffect != null)
        {
            VisualEffect.SetActive(false);
        }
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
