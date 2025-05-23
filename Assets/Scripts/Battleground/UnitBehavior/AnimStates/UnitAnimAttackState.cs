using Unity.VisualScripting;
using UnityEngine;

public class UnitAnimAttackState : StateMachineBehaviour
{
    private AttackController _attackController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attackController = animator.GetComponent<AttackController>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var target = _attackController.Target; 

        if (target == null)
        {
            return;
        }

        LookAtTarget();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_attackController.VisualEffect != null)
        {
            _attackController.VisualEffect.SetActive(false);
        }
    }

    private void LookAtTarget()
    {
        var direction = _attackController.Target.transform.position - _attackController.transform.position;
        _attackController.transform.rotation = Quaternion.LookRotation(direction);
    }
}
