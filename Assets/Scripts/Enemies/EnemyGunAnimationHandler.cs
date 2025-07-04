using Assets.Scripts.SharedLogic;
using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyGunAnimationHandler : MonoBehaviour
    {
        [SerializeField] private AnimatorOverrideController _medievalWeaponControllerOverride;
        [SerializeField] private AnimatorOverrideController _cyberpunkWeaponControllerOverride;
        private Animator _weaponAnimator;

        private const float ShootTime = 0.383f;

        private void Start()
        {
            WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
            //WorldSwapHandler.Instance.OnNewWorldFlicker.AddListener(OnNewWorldFlicker);
            WorldSwapHandler.Instance.OnWorldFlicker.AddListener(OnWorldFlicker);
            _weaponAnimator = GetComponent<Animator>();
            _weaponAnimator.runtimeAnimatorController = _cyberpunkWeaponControllerOverride;

            ShootComponent weaponComp = GetComponent<ShootComponent>();
            weaponComp.OnShootFuture.AddListener(OnShoot);
            weaponComp.OnShootPast.AddListener(OnShoot);
        }

        private void OnWorldSwap()
        {
            if (WorldSwapHandler.Instance.IsInCyberpunkWorld)
            {
                _weaponAnimator.runtimeAnimatorController = _cyberpunkWeaponControllerOverride;
            }
            else
            {
                _weaponAnimator.runtimeAnimatorController = _medievalWeaponControllerOverride;
            }
        }

        private void OnWorldFlicker()
        {
            if (WorldSwapHandler.Instance.IsFlickeringInCyberpunkWorld)
            {
                _weaponAnimator.runtimeAnimatorController = _cyberpunkWeaponControllerOverride;
            }
            else
            {
                _weaponAnimator.runtimeAnimatorController = _medievalWeaponControllerOverride;
            }
        }

        private void OnShoot()
        {
            _weaponAnimator.SetBool("IsShooting", true);
            Invoke("StopShoot", ShootTime);
        }

        private void StopShoot()
        {
            _weaponAnimator.SetBool("IsShooting", false);
        }
    }
}
