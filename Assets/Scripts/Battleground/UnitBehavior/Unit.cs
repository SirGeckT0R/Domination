using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private float _unitHealth;

    [SerializeField] private float _unitMaxHealth = 100f;

    [SerializeField] HealthTracker _healthTracker;

    private IStateMachine _stateMachine;
    public UnitIdleState IdleState { get; private set; }
    public UnitFollowingState FollowingState { get; private set; }
    public UnitAttackingState AttackingState { get; private set; }

    private Animator _animator;
    private NavMeshAgent _agent;

    private void Start()
    {
        UnitSelectionManager.Instance.AddUnit(gameObject);

        _unitHealth = _unitMaxHealth;

        UpdateHealthUI();


        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _stateMachine = new StateMachine();

        IdleState = new UnitIdleState(this, _stateMachine);
        FollowingState = new UnitFollowingState(this, _stateMachine);
        AttackingState = new UnitAttackingState(this, _stateMachine);

        _stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        _stateMachine.CurrentState.HandleInput();
        _stateMachine.CurrentState.Update();

        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
    }

    private void UpdateHealthUI()
    {
        _healthTracker.UpdateSliderValue(_unitHealth, _unitMaxHealth);

        if(_unitHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.RemoveUnit(gameObject);
    }

    public void TakeDamage(int damage)
    {
        _unitHealth -= damage;

        UpdateHealthUI();
    }
}
