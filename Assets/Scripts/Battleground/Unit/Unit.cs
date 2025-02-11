using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private float _unitHealth;

    [SerializeField] private float _unitMaxHealth = 100f;

    [SerializeField] HealthTracker _healthTracker;


    private Animator _animator;
    private NavMeshAgent _agent;

    private void Start()
    {
        UnitSelectionManager.Instance.AddUnit(gameObject);

        _unitHealth = _unitMaxHealth;

        UpdateHealthUI();

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
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
