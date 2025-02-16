using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _agent;
    private DirectionIndicator _directionIndicator;

    [field: SerializeField] public LayerMask Ground { get; private set; }

    public bool IsCommandedToMove { get; set; }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _directionIndicator = GetComponent<DirectionIndicator>();
    }

    public void CommandUnit(RaycastHit hit)
    {
        IsCommandedToMove = true;

        _animator.SetBool("IsMoving", true);
        _agent.SetDestination(hit.point);

        _directionIndicator.DrawLine(hit);
    }

    public void MoveUnit(Transform transform)
    {
        _animator.SetBool("IsMoving", true);
        _agent.SetDestination(transform.position);
    }

    public void StopMovement()
    {
        IsCommandedToMove = false;

        _animator.SetBool("IsMoving", false);
        _agent.isStopped = true;
        _agent.ResetPath();
    }

    private void Update()
    {
        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    IsCommandedToMove = false;
                    _animator.SetBool("IsMoving", false);
                }
            }
        }
    }
}
