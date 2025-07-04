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
            //WorldSwapHandler.Instance.OnNewWorldFlicker.AddListener(OnNewWorldFlicker);
            WorldSwapHandler.Instance.OnWorldFlicker.AddListener(OnWorldFlicker);
            _weaponAnimator = GetComponent<Animator>();
            _weaponAnimator.runtimeAnimatorController = _cyberpunkWeaponControllerOverride;

            ShootComponent weaponComp = GetComponent<ShootComponent>();
            weaponComp.OnShootFuture.AddListener(OnShoot);
            weaponComp.OnShootPast.AddListener(OnShoot);
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

        void OnWorldFlicker()
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
