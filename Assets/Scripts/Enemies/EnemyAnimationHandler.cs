using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyAnimationHandler : MonoBehaviour
    {
        [SerializeField] private AnimatorOverrideController _medievalControllerOverride;
        [SerializeField] private AnimatorOverrideController _cyberpunkControllerOverride;

        [SerializeField] private GameObject _enemyWeapon;
        private Animator _animator;

        private void Start()
        {
            WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
            //WorldSwapHandler.Instance.OnNewWorldFlicker.AddListener(OnNewWorldFlicker);
            WorldSwapHandler.Instance.OnWorldFlicker.AddListener(OnWorldFlicker);

            _animator = GetComponent<Animator>();
            _animator.runtimeAnimatorController = _cyberpunkControllerOverride;

            EnemyAIComponent enemyAi = GetComponent<EnemyAIComponent>();

            enemyAi.OnMovementBegin.AddListener(OnMovementBegin);
            enemyAi.OnMovementEnd.AddListener(OnMovementEnd);
            enemyAi.OnAttackingBegin.AddListener(OnAttackBegin);
            enemyAi.OnAttackingEnd.AddListener(OnAttackEnd);
        }

        private void Update()
        {
        
        }

        private void OnWorldSwap()
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

        private void OnWorldFlicker()
        {
            if (WorldSwapHandler.Instance.IsFlickeringInCyberpunkWorld)
            {
                _animator.runtimeAnimatorController = _cyberpunkControllerOverride;
            }
            else
            {
                _animator.runtimeAnimatorController = _medievalControllerOverride;
            }
        }


        private void OnMovementBegin()
        {
            _animator.SetBool("IsMoving", true);
        }

        private void OnMovementEnd()
        {
            _animator.SetBool("IsMoving", false);
        }

        private void OnAttackBegin()
        {
            _enemyWeapon.SetActive(true);
            _animator.SetBool("IsAttacking", true);
        }

        private void OnAttackEnd()
        {
            _enemyWeapon.SetActive(false);
            _animator.SetBool("IsAttacking", false);
        }
    }
}
