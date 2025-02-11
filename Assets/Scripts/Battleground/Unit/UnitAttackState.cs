using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private AttackController _attackController;

    [SerializeField] private float _stopAttackingDistance = 3f;
    [SerializeField] private float _attackRate = 2f;
    [SerializeField] private float _attackTimer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _navMeshAgent = animator.transform.GetComponent<NavMeshAgent>();
        _attackController = animator.transform.GetComponent<AttackController>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_attackController.AttackTarget != null && !animator.transform.GetComponent<UnitMovement>().IsCommandedToMove)
        {
            LookAtTarget();

            //_navMeshAgent.SetDestination(_attackController.AttackTarget.position);

            if(_attackTimer <= 0)
            {
                Attack();
                _attackTimer = 1f / _attackRate;
            }
            else
            {
                _attackTimer -= Time.deltaTime;
            }

            float distanceToTarget = Vector3.Distance(_attackController.AttackTarget.position, animator.transform.position);
            if (distanceToTarget > _stopAttackingDistance || _attackController.AttackTarget == null)
            {
                animator.SetBool("IsAttacking", false);
            }
        }
        else
        {
            animator.SetBool("IsAttacking", false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attackController.attackEffect.gameObject.SetActive(false);
    }

    private void Attack()
    {
        var damage = _attackController.unitDamage;
        _attackController.AttackTarget.GetComponent<Unit>().TakeDamage(damage);

        SoundManager.Instance.PlayInfantryAttackSound();
    }

    private void LookAtTarget()
    {
        var direction = _attackController.AttackTarget.position - _navMeshAgent.transform.position;
        _navMeshAgent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = _navMeshAgent.transform.eulerAngles.y;
        _navMeshAgent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
