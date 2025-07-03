using Assets.Scripts.SharedLogic;
using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.Miscellaneous
{
    public class HealthPickupComponent : MonoBehaviour
    {
        [SerializeField] private Sprite _futureSprite;
        [SerializeField] private Sprite _pastSprite;

        [SerializeField] private float _healingAmount = 1.0f;
        [SerializeField] private float _spawnPushForce = 3.0f;
        [SerializeField] private bool _isRandomDirection = false;
        public bool IsRandomDirection { set { _isRandomDirection = value; }}
        [SerializeField] private Vector2 _spawnDirection = Vector2.up;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            //var rb = GetComponent<Rigidbody2D>();
            //
            //Vector2 direction;
            //if(_isRandomDirection) direction = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
            //else direction = _spawnDirection.normalized;
            //
            //rb.AddForce(direction * _spawnPushForce, ForceMode2D.Impulse);
        }

        private void OnEnable()
        {
            var rb = GetComponent<Rigidbody2D>();

            Vector2 direction;
            if (_isRandomDirection) direction = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
            else direction = _spawnDirection.normalized;

            rb.AddForce(direction * _spawnPushForce, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                var healthComponent = collision.gameObject.GetComponent<HealthComponent>();

                if (healthComponent != null)
                {
                    healthComponent.Heal(_healingAmount);
                    Destroy(gameObject);
                }
            }
        }

        private void OnWorldSwap()
        {
            if (WorldSwapHandler.Instance.IsInCyberpunkWorld)
            {
                _spriteRenderer.sprite = _futureSprite;
            }
            else
            {
                _spriteRenderer.sprite = _pastSprite;
            }
        }
    }
}
