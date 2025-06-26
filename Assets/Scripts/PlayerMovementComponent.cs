using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovementComponent : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpStrength = 5f;

    private Rigidbody2D _rigidbody;
    private float _inputMoveDirection;
    private bool _isGrounded;

    public UnityEvent OnMovementBegin = new UnityEvent();
    public UnityEvent OnMovementEnd = new UnityEvent(); 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidbody.linearVelocityX = _inputMoveDirection * _speed; // staying away from the y doesnt override gravity
        _isGrounded = Physics2D.Raycast(transform.position, -transform.up, 0.55f, LayerMask.GetMask("Ground")); //TODO: for later maybe two for each side (maybe even getting it from collider width?)

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
            if(context.started) OnMovementBegin?.Invoke();
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.started && _isGrounded)
        {
            _rigidbody.AddForce(transform.up * _jumpStrength, ForceMode2D.Impulse);
        }
    }
}
