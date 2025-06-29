using Assets.Scripts.SharedLogic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerWeaponRotateComponent : MonoBehaviour
    {
        [SerializeField] private Transform _socket;
        private ShootComponent _shootComponent;
        private SpriteRenderer _spriteRenderer;

        private Vector3 _currentPointRotateAround;
        private Vector3 _socketPos;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _shootComponent = GetComponent<ShootComponent>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _currentPointRotateAround = transform.localPosition;
            _socketPos = _socket.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouseWorldPos.z = 0f;

            Vector3 dirToMouse = (mouseWorldPos - transform.parent.position).normalized;
            float angle = Mathf.Atan2(dirToMouse.y, dirToMouse.x) * Mathf.Rad2Deg;

            if (dirToMouse.x < 0)
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
            _shootComponent.SetDirection(dirToMouse);
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

    }
}
