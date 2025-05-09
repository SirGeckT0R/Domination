using UnityEngine;

public class UnitAnimFollowState : StateMachineBehaviour
{
    private AttackController _attackController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attackController = animator.GetComponent<AttackController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_attackController.Target == null || animator.GetBool("IsFollowing"))
        {
            return;
        }

        LookAtTarget();
    }

    private void LookAtTarget()
    {
        _attackController.transform.LookAt(_attackController.Target.transform);
    }
}
