using Assets.Scripts.Battleground.UnitBehavior.LogicStates;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected IStateMachine _stateMachine;
    protected UnitSelectionManager _selectionManager;
    protected Health _health;
    protected Animator _animator;

    public UnitIdleState IdleState { get; protected set; }
    public UnitAttackingState AttackingState { get; protected set; }

    public bool IsDead { get; set; }
    public bool IsSelected { get; set; }
    public int OrderOfSelection { get; set; }

    public void Construct(IStateMachine stateMachine, UnitSelectionManager unitSelectionManager)
    {
        _stateMachine = stateMachine;
        _selectionManager = unitSelectionManager;

        _animator = GetComponent<Animator>();

        _health = GetComponent<Health>();
        _health.OnDeath.AddListener(HandleDeath);
    }

    private void Start()
    {
        _selectionManager.AddUnit(gameObject);

        _stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        if (IsSelected)
        {
            _stateMachine.CurrentState.HandleInput();
        }

        _stateMachine.CurrentState.Update();
    }

    private void HandleDeath()
    {
        _stateMachine.CurrentState.Exit();
        
        _selectionManager.RemoveUnit(gameObject);

        IsSelected = false;
        IsDead = true;
        _animator.SetBool("IsDead", true);
    }

    private void DestroyUnit()
    {
        Destroy(gameObject);
    }
}
