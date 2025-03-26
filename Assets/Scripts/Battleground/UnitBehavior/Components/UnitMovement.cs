using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _agent;
    private DirectionIndicator _directionIndicator;

    [field: SerializeField] public LayerMask Ground { get; private set; }

    public bool IsCommandedToMove { get; set; }
    private bool _isMoving = false;
    [SerializeField] private int _numberOfUnitsInRow = 4;
    [SerializeField] private float _spacing = 3;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _directionIndicator = GetComponent<DirectionIndicator>();
    }

    public void CommandUnit(RaycastHit hit, int offset)
    {
        IsCommandedToMove = true;
        _animator.SetBool("IsMoving", true);

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;

        Vector3 perpendicular = new Vector3(-cameraForward.z, 0, cameraForward.x);

        var side = Mathf.Pow(-1, offset % 2);
        var distance = Mathf.Ceil((offset % _numberOfUnitsInRow) / 2f);
        var depth = offset / _numberOfUnitsInRow;
        var destination = hit.point + perpendicular * _spacing * side * distance;
        destination -= depth * _spacing * destination.normalized;

        _agent.SetDestination(destination);
        _directionIndicator.DrawLine(hit);
    }

    public void MoveUnit(Transform transform)
    {
        _animator.SetBool("IsMoving", true);
        _agent.SetDestination(transform.position);
    }

    public bool IsMoving()
    {
        return _agent.pathPending || _agent.remainingDistance > 0.5f;
    }

    public void StopMovement()
    {
        IsCommandedToMove = false;

        _animator.SetBool("IsMoving", false);
        _agent.isStopped = true;
        _isMoving = false;
        _agent.ResetPath();
    }

    private void Update()
    {
        if (IsCommandedToMove)
        {
            if (_isMoving && !_agent.hasPath)
            {
                _isMoving = false;
                IsCommandedToMove = false;

                Vector3 cameraForward = Camera.main.transform.forward;
                cameraForward.y = 0;
                var rotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = rotation;

                _animator.SetBool("IsMoving", false);
            }

            if (!_agent.hasPath)
            {
                _isMoving = true;

                return;
            }
        }

        _isMoving = false;
    }
}
