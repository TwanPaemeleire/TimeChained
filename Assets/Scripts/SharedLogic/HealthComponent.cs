using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.SharedLogic
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private float _maxHealth = 3.0f;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _flashDuration = 0.1f;
        [SerializeField] private Color _damageFlashColor = Color.red;
        [SerializeField] private Color _healFlashColor = Color.limeGreen;
        [SerializeField] private bool _destroyedOnDeath = true;

        private float _currentHealth;
        private float _invulnerableTimer;
        private Color _originalColor;

        public UnityEvent OnHit = new UnityEvent();
        public UnityEvent OnDeath = new UnityEvent();
        public UnityEvent OnHeal = new UnityEvent();

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;
        }

        private void Update()
        {
            if (_invulnerableTimer > 0.0f)
            {
                _invulnerableTimer -= Time.deltaTime;
            }
        }
    
        public void GetHit(float damage = 1.0f)
        {
            if (_invulnerableTimer > 0.0f)
            {
                return;
            }

            _currentHealth -= damage;
            FlashSprite(_damageFlashColor);
            OnHit?.Invoke();

            _invulnerableTimer = 0.3f;

            if (_currentHealth <= 0.0f)
            {
                Kill();
            }
        }

        public void Kill()
        {
            _currentHealth = 0.0f;
            OnDeath?.Invoke();

            if (_destroyedOnDeath)
            {
                Destroy(gameObject);
            }
        }

        public void Heal(float healAmount)
        {
            FlashSprite(_healFlashColor);
            _currentHealth = Mathf.Clamp(_currentHealth += healAmount, 0, _maxHealth);
            OnHeal?.Invoke();
        }

        public float GetRemainingHealthPercentage()
        {
            return _currentHealth / _maxHealth;
        }

        private void FlashSprite(Color flashColor)
        {
            StopAllCoroutines();
            StartCoroutine(FlashCoroutine(flashColor));
        }

        private IEnumerator FlashCoroutine(Color flashColor)
        {
            _spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(_flashDuration);
            _spriteRenderer.color = _originalColor;
        }
    }
}
