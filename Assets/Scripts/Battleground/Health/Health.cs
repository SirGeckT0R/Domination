using UnityEngine;

public class Health : MonoBehaviour
{
    public float CurrentHealth { get; private set; }

    [field: Header("Settings")]
    [field: SerializeField] public float MaxHealth { get; private set; }

    [field: Header("UI")]
    [SerializeField] private HealthTracker _healthTracker;

    private void Start()
    {
        CurrentHealth = MaxHealth;

        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        _healthTracker.UpdateSliderValue(CurrentHealth, MaxHealth);
    }
}
