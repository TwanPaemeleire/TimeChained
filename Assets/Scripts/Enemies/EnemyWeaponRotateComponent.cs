using Assets.Scripts.SharedLogic;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyWeaponRotateComponent : MonoBehaviour
    {
        [SerializeField] private Transform _socket;
        [SerializeField] private EnemyAIComponent _enemyAIComponent;
        private ShootComponent _shootComponent;
        private SpriteRenderer _spriteRenderer;

        private Vector3 _currentPointRotateAround;
        private Vector3 _socketPos;

        private void Start()
        {
            _shootComponent = GetComponent<ShootComponent>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _currentPointRotateAround = transform.localPosition;
            _socketPos = _socket.localPosition;
        }

        private void Update()
        {
            SetGunDirection(_enemyAIComponent.PlayerDirection);

            _shootComponent.SetDirection(_enemyAIComponent.PlayerDirection);
        }

        public void FlipLeft()
        {
            transform.localPosition = new Vector3(-_currentPointRotateAround.x, _currentPointRotateAround.y);
            _socket.localPosition = new Vector3(-_socketPos.x, _socketPos.y);
        }

        public void FlipRight()
        {
            transform.localPosition = _currentPointRotateAround;
            _socket.localPosition = _socketPos;
        }

        private void SetGunDirection(Vector3 dirToPoint)
        {
            float angle = Mathf.Atan2(dirToPoint.y, dirToPoint.x) * Mathf.Rad2Deg;

            if (dirToPoint.x < 0)
            {
                _spriteRenderer.flipX = true;
                _socket.localPosition = new Vector3(-_socketPos.x, _socketPos.y);
                angle -= 180f;
            }
            else
            {
                _spriteRenderer.flipX = false;
                _socket.localPosition = _socketPos;
            }

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
