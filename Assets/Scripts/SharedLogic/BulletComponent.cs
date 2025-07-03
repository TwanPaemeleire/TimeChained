using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.SharedLogic
{
    public class BulletComponent : MonoBehaviour
    {
        [SerializeField] private float _maxLifetime = 3.0f;
        [SerializeField] private Sprite _cyberpunkBulletSprite;
        [SerializeField] private Sprite _medievalBulletSprite;
        private BulletType _bulletType;
        public BulletType BulletType { set { _bulletType = value; } }
        private SpriteRenderer _bulletRenderer;

        protected float _speed = 6f;
        protected float _startSpeed = 6f;
        private string _shooterTag;
        private bool _shotFromTrap = false;
        private bool _isBeingReturnedToPool = false;

        private void Start()
        {
            _bulletRenderer = GetComponent<SpriteRenderer>();
            OnWorldSwap();
        }

        private void OnEnable()
        {
            _bulletRenderer = GetComponent<SpriteRenderer>();
            OnWorldSwap();
        }

        public virtual void Initialize()
        {
            _isBeingReturnedToPool = false;
            WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
            Invoke(nameof(DestroyBullet), _maxLifetime);
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            transform.position += _speed * Time.deltaTime * transform.right;
        }

        public void SetSpeed(float newSpeed)
        {
            _speed = newSpeed;
        }

        public void SetShooterTag(string tag)
        {
            _shooterTag = tag;
            if (tag == "Trap") _shotFromTrap = true; //setting it here so not continuously checking in OnTriggerEnter
            else _shotFromTrap = false; //NEED TO DO THIS, memory pool doesnt reset bool, so need to reset every time new tag gets set
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_shooterTag) || collision.CompareTag(transform.tag) || collision.CompareTag("PatrolPoint") || collision.CompareTag("OneWayPlatform") || (collision.CompareTag("Enemy") && _shotFromTrap)) return; //hits own shooter or other bullet, trap shooters cannot hit enemies

            if (collision.CompareTag("Player") || collision.CompareTag("Enemy") || collision.CompareTag("Boss")) 
            {
                var healthComponent = collision.gameObject.GetComponent<HealthComponent>();

                if (healthComponent != null)
                {
                    healthComponent.GetHit();
                }

                //transform.gameObject.SetActive(false);
            }
            DestroyBullet();
        }

        protected virtual void DestroyBullet()
        {
            if(_isBeingReturnedToPool) return;
            _isBeingReturnedToPool = true;

            CancelInvoke(nameof(DestroyBullet));
            _speed = _startSpeed;
            WorldSwapHandler.Instance.OnWorldSwap.RemoveListener(OnWorldSwap);
            BulletsHandler.Instance.ReturnBullet(_bulletType, this.gameObject);
        }

        private void OnWorldSwap()
        {
            if(WorldSwapHandler.Instance.IsInCyberpunkWorld)
            {
                _bulletRenderer.sprite = _cyberpunkBulletSprite;
            }
            else
            {
                _bulletRenderer.sprite = _medievalBulletSprite;
            }
        }
    }
}
