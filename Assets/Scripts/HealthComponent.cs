using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 3.0f;
    private float _currentHealth;

    public UnityEvent OnHit = new UnityEvent();
    public UnityEvent OnDeath = new UnityEvent();
    public UnityEvent OnHeal = new UnityEvent();

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }
    
    public void GetHit(float damage = 1.0f)
    {
        _currentHealth -= damage;
        OnHit?.Invoke();
        if(_currentHealth <= 0.0f)
        {
            _currentHealth = 0.0f;
            OnDeath?.Invoke();
        }
    }

    public void Kill()
    {
        _currentHealth = 0.0f;
        OnDeath?.Invoke();
    }

    public void Heal(float healAmount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth += healAmount, 0, _maxHealth);
        OnHeal?.Invoke();
    }

    public float GetRemainingHealthPercentage()
    {
        return _currentHealth / _maxHealth;
    }
}
