using UnityEngine;

public class UnitAnimIdleState : StateMachineBehaviour
{
    private AttackController _attackController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //_attackController = animator.transform.GetComponent<AttackController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (_attackController.AttackTarget != null)
        //{
        //    animator.SetBool("IsFollowing", true);
        //}
    }
}
