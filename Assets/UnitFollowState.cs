using UnityEngine;
using UnityEngine.AI;

public class UnitFollowState : StateMachineBehaviour
{
    private AttackController _attackController;

    private NavMeshAgent _navMeshAgent;

    [SerializeField] private float _attackDistance = 1f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attackController = animator.GetComponent<AttackController>();
        _navMeshAgent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_attackController.AttackTarget == null)
        {
            animator.SetBool("IsFollowing", false);
        }
        else
        {
            if (!animator.transform.GetComponent<UnitMovement>().IsCommandedToMove)
            {

                _navMeshAgent.SetDestination(_attackController.AttackTarget.position);
                animator.transform.LookAt(_attackController.AttackTarget);

                //float distanceToTarget = Vector3.Distance(_attackController.AttackTarget.position, animator.transform.position);
                //if (distanceToTarget < _attackDistance)
                //{
                //    _navMeshAgent.SetDestination(animator.transform.position);
                //    animator.SetBool("IsAttacking", true);
                //}
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _navMeshAgent.SetDestination(animator.transform.position);
    }
}
