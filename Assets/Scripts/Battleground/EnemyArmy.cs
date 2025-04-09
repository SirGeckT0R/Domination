using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Battleground
{
    public class EnemyArmy : Army
    {
        [field: SerializeField] public List<PatrolRoute> PatrolRoutes { get; private set; }
        private int  _currentPatrolRouteIndex = 0;

        public override void AddUnit(Unit unit)
        {
            base.AddUnit(unit);
        }

        public void SetTarget(Transform transform)
        {
            foreach (var unit in Units)
            {
                var unitMovement = unit.GetComponent<UnitMovement>();
                if (unitMovement != null)
                {
                    unitMovement.MoveUnit(transform);
                }
            }
        }

        public void StartPatrolling()
        {
            foreach (var unit in Units)
            {
                var patrol = unit.GetComponent<Patrol>();
                if(patrol != null)
                {
                    var route = PatrolRoutes[_currentPatrolRouteIndex];
                    patrol.SetPatrolRoute(route);
                    _currentPatrolRouteIndex = (_currentPatrolRouteIndex + 1) % PatrolRoutes.Count;
                }
            }
        }
    }
}
