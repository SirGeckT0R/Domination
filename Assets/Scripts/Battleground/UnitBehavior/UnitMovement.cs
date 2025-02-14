using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private Camera _camera;
    private Animator _animator;
    private NavMeshAgent _agent; 
    private DirectionIndicator _directionIndicator;

    [SerializeField] private LayerMask _ground;    
    public bool IsCommandedToMove { get; private set; }

    void Start()
    {
        _camera = Camera.main;

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _directionIndicator = GetComponent<DirectionIndicator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _ground))
            {
                IsCommandedToMove = true;
                _animator.SetBool("IsMoving", true);
                _agent.SetDestination(hit.point);

                _directionIndicator.DrawLine(hit);
            }
        }
        //think about the purpose of isCommandedToMove - too much confusion and it stands in the way of other scripts
        //agent.haspath randomly becomes false and alse check why the unit moves to the point where you chose to attack a target iven though the target moved already
        if (!_agent.hasPath || _agent.remainingDistance <= _agent.stoppingDistance)
        {
            IsCommandedToMove = false;

            _animator.SetBool("IsMoving", false);
        }
    }
}
