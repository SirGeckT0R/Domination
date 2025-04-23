using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private UnitSelectionManager _unitSelectionManager;
    [SerializeField] private BattleUIManager _battleUIManager;
    public override void InstallBindings()
    {
        Container.Bind<IStateMachine>().To<StateMachine>().FromNew().AsTransient();

        Container.Bind<UnitSelectionManager>().FromInstance(_unitSelectionManager).AsSingle();
        Container.Bind<BattleUIManager>().FromInstance(_battleUIManager).AsSingle();
    }
}