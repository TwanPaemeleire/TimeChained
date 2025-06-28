using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        [SerializeField] private AnimatorOverrideController _medievalControllerOverride;
        [SerializeField] private AnimatorOverrideController _cyberpunkControllerOverride;
        private Animator _animator;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
            WorldSwapHandler.Instance.OnNewWorldFlicker.AddListener(OnNewWorldFlicker);
            WorldSwapHandler.Instance.OnCurrentWorldBackFlicker.AddListener(OnCurrentWorldBackFlicker);
            _animator = GetComponent<Animator>();
            _animator.runtimeAnimatorController = _cyberpunkControllerOverride;

            PlayerMovementComponent playerMovement = GetComponent<PlayerMovementComponent>();
            playerMovement.OnMovementBegin.AddListener(OnMovementBegin);
            playerMovement.OnMovementEnd.AddListener(OnMovementEnd);
            playerMovement.OnJumpBegin.AddListener(OnJumpBegin);
            playerMovement.OnJumpEnd.AddListener(OnJumpEnd);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnWorldSwap()
        {
            if (WorldSwapHandler.Instance.IsInCyberpunkWorld)
            {
                _animator.runtimeAnimatorController = _cyberpunkControllerOverride;
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
                _animator.runtimeAnimatorController = _cyberpunkControllerOverride;
            }
        }

        void OnCurrentWorldBackFlicker()
        {
            if (WorldSwapHandler.Instance.IsInCyberpunkWorld)
            {
                _animator.runtimeAnimatorController = _cyberpunkControllerOverride;
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

        void OnJumpBegin()
        {
            _animator.SetBool("IsJumping", true);
        }
        void OnJumpEnd()
        {
            _animator.SetBool("IsJumping", false);
        }
    }
}
