using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] private PatrolRoute _route;

    private List<Transform> _points;
    private int destPoint = 0;
    private UnitMovement _unitMovement;

    private void Start()
    {
        _unitMovement = GetComponent<UnitMovement>();
        SetPatrolRoute(_route);
    }

    public void SetPatrolRoute(PatrolRoute patrolRoute)
    {
        if (patrolRoute != null)
        {
            _points = patrolRoute.Nodes;
        }
    }

    void GotoNextPoint()
    {
        if (_points.Count == 0)
        {
            return;
        }

        _unitMovement.MoveUnit(_points[destPoint]);

        destPoint = (destPoint + 1) % _points.Count;
    }

    void Update()
    {
        if (!_unitMovement.IsMoving() && _points != null)
        {
            GotoNextPoint();
        }
    }
}
