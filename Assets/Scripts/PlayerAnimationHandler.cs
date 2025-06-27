using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private AnimatorOverrideController _medievalControllerOverride;
    private RuntimeAnimatorController _defaultController;
    private Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
        WorldSwapHandler.Instance.OnNewWorldFlicker.AddListener(OnNewWorldFlicker);
        WorldSwapHandler.Instance.OnCurrentWorldBackFlicker.AddListener(OnCurrentWorldBackFlicker);
        _animator = GetComponent<Animator>();
        _defaultController = _animator.runtimeAnimatorController;

        PlayerMovementComponent playerMovement = GetComponent<PlayerMovementComponent>();
        playerMovement.OnMovementBegin.AddListener(OnMovementBegin);
        playerMovement.OnMovementEnd.AddListener(OnMovementEnd);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnWorldSwap()
    {
        if (WorldSwapHandler.Instance.IsInCyberpunkWorld)
        {
            _animator.runtimeAnimatorController = _defaultController;
        }
        else
        {
            _animator.runtimeAnimatorController = _medievalControllerOverride;
        }
    }

    void OnNewWorldFlicker()
    {
        if (WorldSwapHandler.Instance.IsInCyberpunkWorld)
        {
            _animator.runtimeAnimatorController = _medievalControllerOverride;
        }
        else
        {
            _animator.runtimeAnimatorController = _defaultController;
        }
    }

    void OnCurrentWorldBackFlicker()
    {
        if (WorldSwapHandler.Instance.IsInCyberpunkWorld)
        {
            _animator.runtimeAnimatorController = _defaultController;
        }
        else
        {
            _animator.runtimeAnimatorController = _medievalControllerOverride;
        }
    }

    void OnMovementBegin()
    {
        _animator.SetBool("IsMoving", true);
    }

    void OnMovementEnd()
    {
        _animator.SetBool("IsMoving", false);
    }
}
