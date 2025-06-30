using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.SharedLogic
{
    public class BulletComponent : MonoBehaviour
    {
        [SerializeField] private float _maxLifetime = 3.0f;
        [SerializeField] private Sprite _cyberpunkBulletSprite;
        [SerializeField] private Sprite _medievalBulletSprite;
        private SpriteRenderer _bulletRenderer;

        private float _speed = 6f;
        private string _shooterTag;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _bulletRenderer = GetComponent<SpriteRenderer>();
            WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
            Initialize();
        }

        public void Initialize()
        {
            Invoke(nameof(DestroyBullet), _maxLifetime);
        }

        // Update is called once per frame
        void Update()
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
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_shooterTag) || collision.CompareTag(transform.tag)) return; //hits own shooter or other bullet
            if(collision.CompareTag("Player"))
            {
                Debug.Log("Player hit");
                transform.gameObject.SetActive(false);
            }
            DestroyBullet();
        }

        void DestroyBullet()
        {
            CancelInvoke(nameof(DestroyBullet));
            BulletsHandler.Instance.ReturnBullet(this.gameObject);
        }

        void OnWorldSwap()
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
