using Assets.Scripts.SharedLogic;
using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerGunAnimationHandler : MonoBehaviour
    {
        [SerializeField] private AnimatorOverrideController _medievalWeaponControllerOverride;
        [SerializeField] private AnimatorOverrideController _cyberpunkWeaponControllerOverride;
        private Animator _weaponAnimator;

        private const float ShootTime = 0.3f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
            WorldSwapHandler.Instance.OnNewWorldFlicker.AddListener(OnNewWorldFlicker);
            WorldSwapHandler.Instance.OnCurrentWorldBackFlicker.AddListener(OnCurrentWorldBackFlicker);
            _weaponAnimator = GetComponent<Animator>();
            _weaponAnimator.runtimeAnimatorController = _cyberpunkWeaponControllerOverride;

            ShootComponent weaponComp = GetComponent<ShootComponent>();
            weaponComp.OnShoot.AddListener(OnShoot);
        }

        void OnWorldSwap()
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

        void OnNewWorldFlicker()
        {
            if (WorldSwapHandler.Instance.IsInCyberpunkWorld)
            {
                _weaponAnimator.runtimeAnimatorController = _medievalWeaponControllerOverride;
            }
            else
            {
                _weaponAnimator.runtimeAnimatorController = _cyberpunkWeaponControllerOverride;
            }
        }

        void OnCurrentWorldBackFlicker()
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

        void OnShoot()
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
