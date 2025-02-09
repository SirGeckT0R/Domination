using UnityEngine;

public class UnitIdleState : StateMachineBehaviour
{
    private AttackController _attackController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attackController = animator.transform.GetComponent<AttackController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_attackController.AttackTarget != null)
        {
            animator.SetBool("IsFollowing", true);
        }
    }
}
