using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private Camera _camera;
    private NavMeshAgent _agent; 
    private DirectionIndicator _directionIndicator;

    [SerializeField] private LayerMask _ground;    
    public bool IsCommandedToMove { get; private set; }

    void Start()
    {
        _camera = Camera.main;
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
                _agent.SetDestination(hit.point);

                _directionIndicator.DrawLine(hit);
            }
        }

        if(!_agent.hasPath || _agent.remainingDistance == _agent.stoppingDistance)
        {
            IsCommandedToMove = false;
        }
    }
}
