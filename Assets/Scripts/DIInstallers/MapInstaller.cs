using Assets.Scripts.Map.AI.Considerations;
using Assets.Scripts.Map.Managers;
using UnityEngine;
using Zenject;

public class MapInstaller : MonoInstaller
{
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private CountyManager _countyManager;
    public override void InstallBindings()
    {
        Container.Bind<TurnManager>().FromInstance(_turnManager).AsSingle();
        Container.Bind<CountyManager>().FromInstance(_countyManager).AsSingle();
    }
}
