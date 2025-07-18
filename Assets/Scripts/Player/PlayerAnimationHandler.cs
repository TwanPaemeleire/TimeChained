using Assets.Scripts.World;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        [SerializeField] private AnimatorOverrideController _medievalControllerOverride;
        [SerializeField] private AnimatorOverrideController _cyberpunkControllerOverride;
        private Animator _animator;
        private bool _isEnteringBossAerena = false;
        public bool IsEnteringBossArena { set { _isEnteringBossAerena = value; } }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
            //WorldSwapHandler.Instance.OnNewWorldFlicker.AddListener(OnNewWorldFlicker);
            WorldSwapHandler.Instance.OnWorldFlicker.AddListener(OnWorldFlicker);
            _animator = GetComponent<Animator>();
            _animator.runtimeAnimatorController = _cyberpunkControllerOverride;

            PlayerMovementComponent playerMovement = GetComponent<PlayerMovementComponent>();
            playerMovement.OnMovementBegin.AddListener(OnMovementBegin);
            playerMovement.OnMovementEnd.AddListener(OnMovementEnd);
            playerMovement.OnJumpBegin.AddListener(OnJumpBegin);
            playerMovement.OnJumpEnd.AddListener(OnJumpEnd);
            playerMovement.OnFallBegin.AddListener(OnFallBegin);
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

        void OnWorldFlicker()
        {
            if (WorldSwapHandler.Instance.IsFlickeringInCyberpunkWorld)
            {
                _animator.runtimeAnimatorController = _cyberpunkControllerOverride;
            }
            else
            {
                _animator.runtimeAnimatorController = _medievalControllerOverride;
            }

            Debug.Log("Changed after world back");
        }

        void OnMovementBegin()
        {
            _animator.SetBool("IsMoving", true);
        }

        void OnMovementEnd()
        {
            if (_isEnteringBossAerena) return;
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

        void OnFallBegin()
        {
            _animator.SetTrigger("FallTrigger");
            _animator.SetBool("IsJumping", true);
        }
    }
}
