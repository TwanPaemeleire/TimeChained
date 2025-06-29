using Assets.Scripts.SharedLogic;
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
        private float _inputMoveDirection;
        private bool _isGrounded;
        private bool _canDoubleJump;
        private bool _isJumping;

        private ShootComponent _shootComponent;
        private PlayerWeaponRotateComponent _weaponRotateComponent;

        public UnityEvent OnMovementBegin = new UnityEvent();
        public UnityEvent OnMovementEnd = new UnityEvent();

        public UnityEvent OnJumpBegin = new UnityEvent();
        public UnityEvent OnJumpEnd = new UnityEvent();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _shootComponent = GetComponentInChildren<ShootComponent>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _weaponRotateComponent = GetComponentInChildren<PlayerWeaponRotateComponent>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _rigidbody.linearVelocityX = _inputMoveDirection * _speed; // staying away from the y doesnt override gravity
            _isGrounded = Physics2D.Raycast(transform.position, -transform.up, 0.55f, LayerMask.GetMask("Ground")); //TODO: for later maybe two for each side (maybe even getting it from collider width?)
            if(_isJumping && _isGrounded && _rigidbody.linearVelocityY <= 0f)
            {
                _isJumping = false;
                OnJumpEnd?.Invoke();
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            if(context.canceled)
            {
                _inputMoveDirection = 0.0f;
                OnMovementEnd?.Invoke();
            }
            else
            {
                _inputMoveDirection = context.ReadValue<float>();
                if(_inputMoveDirection < 0.0f)
                {
                    _spriteRenderer.flipX = true;
                    _weaponRotateComponent.FlipLeft();
                }
                else
                {
                    _spriteRenderer.flipX = false;
                    _weaponRotateComponent.FlipRight();
                }
                if(context.started) OnMovementBegin?.Invoke();
            }
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                if (_isGrounded)
                {
                    _rigidbody.AddForce(transform.up * _jumpStrength, ForceMode2D.Impulse);
                    _canDoubleJump = true;
                    _isJumping = true;
                    OnJumpBegin?.Invoke();
                }
                else if(_canDoubleJump)
                {
                    _rigidbody.linearVelocityY = 0; // otherwise jump immediately after first jump would go super high
                    _rigidbody.AddForce(transform.up * _jumpStrength, ForceMode2D.Impulse);
                    _canDoubleJump = false;
                }
            }


        }
    }
}
