using Assets.Scripts.World;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scripts.SharedLogic
{
    public class ShootComponent : MonoBehaviour
    {
        [SerializeField] private float _fireRate;
        [SerializeField] private bool _shouldShoot; //for eg traps that just always shoot
        [SerializeField] private Vector3 _direction = new Vector3(1, 0, 0);
        [SerializeField] private Transform _socket;
        [SerializeField] private BulletType _bulletType;
        private float _accumulatedTime;

        public UnityEvent OnShootFuture = new UnityEvent();
        public UnityEvent OnShootPast = new UnityEvent();

        void Update()
        {
            if (_accumulatedTime <= _fireRate) //not necessary to keep increasing if it is already bigger than it needs to be
            {
                _accumulatedTime += Time.deltaTime;
            }
            else
            {
                if (_shouldShoot)
                {
                    Shoot();
                    _accumulatedTime -= _fireRate;
                }
            }
        }

        private void Shoot()
        {
            GameObject bullet = BulletsHandler.Instance.RequestBullet(_bulletType);

            if (bullet != null)
            {    
                bullet.transform.position = _socket.position;
                bullet.transform.right = _direction.normalized;
                BulletComponent bulletComp = bullet.GetComponent<BulletComponent>();
                bulletComp.SetShooterTag(transform.tag);
                bulletComp.BulletType = _bulletType;

                if (WorldSwapHandler.Instance.IsInCyberpunkWorld)
                {
                    OnShootFuture?.Invoke();
                }
                else
                {
                    OnShootPast?.Invoke();
                }
            }
        }

        public void Shoot(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _shouldShoot = true;
            }
            else if (context.canceled)
            {
                _shouldShoot = false;
            }
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void SetShouldShoot(bool newShouldShoot)
        {
            _shouldShoot = newShouldShoot;
        }
    }
}
