using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.PlayerInput;
using UnityEngine;
using Zenject;

public class MapInstaller : MonoInstaller
{
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private CountyManager _countyManager;
    [SerializeField] private PlayerInputManager _inputManager;
    [SerializeField] private UIManager _uiManager;
    public override void InstallBindings()
    {
        Container.Bind<TurnManager>().FromInstance(_turnManager).AsSingle();
        Container.Bind<CountyManager>().FromInstance(_countyManager).AsSingle();
        Container.Bind<PlayerInputManager>().FromInstance(_inputManager).AsSingle();
        Container.Bind<UIManager>().FromInstance(_uiManager).AsSingle();
    }
}
