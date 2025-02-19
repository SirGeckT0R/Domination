using Assets.Scripts.Map.Managers;
using UnityEngine;
using Zenject;

public class MapInstaller : MonoInstaller
{
    [SerializeField] private TurnManager _turnManager;
    public override void InstallBindings()
    {
        Container.Bind<TurnManager>().FromInstance(_turnManager).AsSingle();
    }
}
