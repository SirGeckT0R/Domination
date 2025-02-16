using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private UnitSelectionManager _unitSelectionManager;
    public override void InstallBindings()
    {
        Container.Bind<IStateMachine>().To<StateMachine>().FromNew().AsTransient();

        Container.Bind<UnitSelectionManager>().FromInstance(_unitSelectionManager).AsSingle();
    }
}