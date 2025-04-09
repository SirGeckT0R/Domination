using Assets.Scripts;
using Assets.Scripts.Battleground;
using Assets.Scripts.Battleground.BattleGoals;
using Assets.Scripts.Battleground.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject _knightUnitPrefab;
    [SerializeField] private GameObject _wallUnitPrefab;
    [SerializeField] private GameObject _enemyUnitPrefab;
    [SerializeField] private GameObject _enemyWallUnitPrefab;

    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Transform _playerWallSpawnPoint;
    [SerializeField] private Transform _enemySpawnPoint;
    [SerializeField] private Transform _enemyWallSpawnPoint;

    [SerializeField] private Army _playerArmy;
    [SerializeField] private EnemyArmy _enemyArmy;

    [field: SerializeField] private DataHolder _dataHolder;
    [field: SerializeField] private WarInfo _warInfo;

    private List<BattleGoal> _playerGoals = new List<BattleGoal>();
    private List<BattleGoal> _enemyGoals = new List<BattleGoal>();

    private DiContainer _diContainer;

    [Inject]
    public void Construct(DiContainer diContainer)
    {
        _diContainer = diContainer;
        _dataHolder = DataHolder.Instance;

        if(_dataHolder != null && _dataHolder.CurrentWarInfo != null)
        {
            _warInfo = _dataHolder.CurrentWarInfo;
        }
    }

    private void Start()
    {
        SpawnPlayerTeam();

        SpawnEnemyTeam();

        SpawnWall();

        ListenToBattleGoals();
    }

    private void SpawnWall()
    {
        ArmyDefeatedGoal goal;
        switch (_warInfo.BattleType)
        {
            case BattleType.Defend:
                _diContainer.InstantiatePrefab(_wallUnitPrefab, _playerWallSpawnPoint.position, _playerWallSpawnPoint.rotation, _playerArmy.transform);
                StartCoroutine(EnemyArmyAttack());
                goal = _enemyArmy.AddComponent<ArmyDefeatedGoal>();
                goal.AchievedBy = BattleOpponent.Player;
                goal.Initialize();

                break;
            case BattleType.Attack:
                _diContainer.InstantiatePrefab(_enemyWallUnitPrefab, _enemyWallSpawnPoint.position, _enemyWallSpawnPoint.rotation, _enemyArmy.transform);
                StartCoroutine(EnemyArmyPatrol());
                goal = _playerArmy.AddComponent<ArmyDefeatedGoal>();
                goal.AchievedBy = BattleOpponent.Enemy;
                goal.Initialize();

                break;
        }
    }

    private IEnumerator EnemyArmyAttack()
    {
        yield return new WaitForSeconds(3f);
        _enemyArmy.SetTarget(_playerWallSpawnPoint);
    }

    private IEnumerator EnemyArmyPatrol()
    {
        yield return new WaitForSeconds(3f);
        _enemyArmy.StartPatrolling();
    }

    private void ListenToBattleGoals()
    {
        var battleGoals = FindObjectsByType<BattleGoal>(FindObjectsSortMode.None);

        foreach(var goal in battleGoals)
        {
            switch (goal.AchievedBy)
            {
                case BattleOpponent.Player:
                    _playerGoals.Add(goal);

                    break;
                case BattleOpponent.Enemy:
                    _enemyGoals.Add(goal);

                    break;
            }

            goal.OnGoalAchieved.AddListener(HandleGoalAchieved);
        }
    }

    private void SpawnPlayerTeam()
    {
        var warriorsCount = _warInfo.PlayerWarriorsCount;
        var newPosition = _playerSpawnPoint.position;
        var newRotation = _playerSpawnPoint.rotation;
        for (int i = 0; i < warriorsCount; i++)
        {
            var unit = _diContainer.InstantiatePrefab(_knightUnitPrefab, newPosition, newRotation, _playerArmy.transform).GetComponent<Unit>();
            _playerArmy.AddUnit(unit);
            newPosition.x += 1;
            newPosition.z += 1;
        }
    }

    private void SpawnEnemyTeam()
    {
        var warriorsCount = _warInfo.EnemyWarriorsCount;
        var newPosition = _enemySpawnPoint.position;
        var newRotation = _enemySpawnPoint.rotation;
        for (int i = 0; i < warriorsCount; i++)
        {
            var unit = _diContainer.InstantiatePrefab(_enemyUnitPrefab, newPosition, newRotation, _enemyArmy.transform).GetComponent<Unit>();
            _enemyArmy.AddUnit(unit);
            newPosition.x += 1;
            newPosition.z += 1;
        }
    }

    private void HandleGoalAchieved(GoalAchievedInfo goalInfo)
    {
        Debug.Log("Goal achieved by" + goalInfo.AchievedBy.ToString());
        switch(goalInfo.AchievedBy)
        {
            case BattleOpponent.Player:
                goalInfo.Goal.OnGoalAchieved.RemoveListener(HandleGoalAchieved);
                _playerGoals.Remove(goalInfo.Goal);
                if (_playerGoals.Count == 0)
                {
                    Debug.Log("Player has achieved all of it's goals");
                    var warResult = ScriptableObject.CreateInstance<WarResult>();
                    warResult.Initialize(
                        BattleOpponent.Player,
                        remainingPlayerWarriorsCount: _playerArmy.WarriorsCount,
                        remainingEnemyWarriorsCount: _enemyArmy.WarriorsCount
                    );
                    if (_dataHolder != null)
                    {
                        _dataHolder.CurrentWarResult = warResult;
                    }

                    StartCoroutine(EndBattle());
                }

                break;
            case BattleOpponent.Enemy:
                goalInfo.Goal.OnGoalAchieved.RemoveListener(HandleGoalAchieved);
                _enemyGoals.Remove(goalInfo.Goal);
                if (_enemyGoals.Count == 0)
                {
                    Debug.Log("Enemy has achieved all of it's goals");
                    var warResult = ScriptableObject.CreateInstance<WarResult>();
                    warResult.Initialize(
                        BattleOpponent.Enemy,
                        remainingPlayerWarriorsCount: _playerArmy.WarriorsCount,
                        remainingEnemyWarriorsCount: _enemyArmy.WarriorsCount
                    );
                    if(_dataHolder != null)
                    {
                        _dataHolder.CurrentWarResult = warResult;

                    }

                    SceneManager.LoadScene("Map");
                }

                break;
        }
    }

    private IEnumerator EndBattle()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Map");
    }
}
