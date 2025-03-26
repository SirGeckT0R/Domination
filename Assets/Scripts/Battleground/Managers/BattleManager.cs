using Assets.Scripts;
using Assets.Scripts.Battleground.BattleGoals;
using UnityEngine;
using Zenject;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject _knightUnitPrefab;
    [SerializeField] private GameObject _enemyUnitPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Transform _enemySpawnPoint;
    [SerializeField] private Transform _playerTeam;
    [SerializeField] private Transform _enemyTeam;
    [SerializeField] private WarInfo _warInfo;

    private DiContainer _diContainer;

    [Inject]
    public void Construct(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }

    private void Start()
    {
        SpawnPlayerTeam();

        SpawnEnemyTeam();

        ListenToBattleGoals();
    }

    private void ListenToBattleGoals()
    {
        var battleGoals = FindObjectsByType<BattleGoal>(FindObjectsSortMode.None);

        foreach(var goal in battleGoals)
        {
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
            _diContainer.InstantiatePrefab(_knightUnitPrefab, newPosition, newRotation, _playerTeam);
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
            _diContainer.InstantiatePrefab(_enemyUnitPrefab, newPosition, newRotation, _enemyTeam);
            newPosition.x += 1;
            newPosition.z += 1;
        }
    }

    private void HandleGoalAchieved()
    {
        Debug.Log("Goal achieved");
    }
}
