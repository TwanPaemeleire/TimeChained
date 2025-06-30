using Assets.Scripts.SharedLogic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerMovementComponent : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _jumpStrength = 5f;

        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;
        private Vector2 _inputMoveDirection;
        private Vector2 _previousInputMoveDirection;
        private bool _isGrounded;
        private bool _canDoubleJump;
        private bool _isJumping;

        private ShootComponent _shootComponent;
        private PlayerWeaponRotateComponent _weaponRotateComponent;

        private GameObject _currentOneWayPlatform = null;
        private BoxCollider2D _boxCollider;

        public UnityEvent OnMovementBegin = new UnityEvent();
        public UnityEvent OnMovementEnd = new UnityEvent();

        public UnityEvent OnJumpBegin = new UnityEvent();
        public UnityEvent OnJumpEnd = new UnityEvent();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _shootComponent = GetComponentInChildren<ShootComponent>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _weaponRotateComponent = GetComponentInChildren<PlayerWeaponRotateComponent>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _rigidbody.linearVelocityX = _inputMoveDirection.x * _speed; // staying away from the y doesnt override gravity
            _isGrounded = Physics2D.Raycast(transform.position, -transform.up, 0.55f, LayerMask.GetMask("Ground")); //TODO: for later maybe two for each side (maybe even getting it from collider width?)
            if(_isJumping && _isGrounded && _rigidbody.linearVelocityY <= 0f)
            {
                _isJumping = false;
                OnJumpEnd?.Invoke();
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            _previousInputMoveDirection = _inputMoveDirection;
            if(context.canceled)
            {
                _inputMoveDirection = Vector2.zero;
                OnMovementEnd?.Invoke();
            }
            else
            {
                _inputMoveDirection = context.ReadValue<Vector2>();
                if(_inputMoveDirection.x < 0.0f)
                {
                    _spriteRenderer.flipX = true;
                    _weaponRotateComponent.FlipLeft();
                }
                else
                {
                    _spriteRenderer.flipX = false;
                    _weaponRotateComponent.FlipRight();
                }

                if (_previousInputMoveDirection.x == 0.0f && _inputMoveDirection.x != 0.0f) OnMovementBegin?.Invoke();
                if(_inputMoveDirection.y < 0.0f && !(_previousInputMoveDirection.y < 0.0f)) // Player wants to drop down through platform
                {
                     StartCoroutine(DisableOneWayCollisions());
                }
            }
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (_isGrounded)
                {
                    _rigidbody.AddForce(transform.up * _jumpStrength, ForceMode2D.Impulse);
                    _canDoubleJump = true;
                    _isJumping = true;
                    OnJumpBegin?.Invoke();
                }
                else if (_canDoubleJump)
                {
                    _rigidbody.linearVelocityY = 0; // otherwise jump immediately after first jump would go super high
                    _rigidbody.AddForce(transform.up * _jumpStrength, ForceMode2D.Impulse);
                    _canDoubleJump = false;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("OneWayPlatform"))
            {
                _currentOneWayPlatform = collision.gameObject;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("OneWayPlatform"))
            {
                _currentOneWayPlatform = null;
            }
        }

        IEnumerator DisableOneWayCollisions()
        {
            BoxCollider2D oneWayPlatformCollider = _currentOneWayPlatform.GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(_boxCollider, oneWayPlatformCollider);
            _rigidbody.AddForce(new Vector2(0, -5.0f), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.25f);
            Physics2D.IgnoreCollision(_boxCollider, oneWayPlatformCollider, false);
        }
    }
}
