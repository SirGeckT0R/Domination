using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    [field: Header("Settings")]
    [field: SerializeField] public float MaxHealth { get; private set; }

    [field: Header("UI")]
    [SerializeField] private HealthTracker _healthTracker;

    public UnityEvent OnTakingDamage;
    public UnityEvent OnDeath;

    private void Start()
    {
        CurrentHealth = MaxHealth;

        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
        {
            return;
        }

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
            StartCoroutine(DestroyGameObject());
        }

        OnTakingDamage?.Invoke();

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        _healthTracker.UpdateSliderValue(CurrentHealth, MaxHealth);
    }

    private IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(2f);
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
