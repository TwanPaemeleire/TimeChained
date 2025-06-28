using System;
using Assets.Scripts.World;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.SharedLogic
{
    public class BulletsHandler : MonoSingleton<BulletsHandler>
    {
        [SerializeField] private int _maxBullets = 10;
        [SerializeField] private GameObject _bulletPrefab;
        private ObjectPool<GameObject> _bulletPool;

        private void Start()
        {
            _bulletPool = new ObjectPool<GameObject>(CreateBullet, OnTakeBulletFromPool, OnBulletReturnedToPool, null, true, _maxBullets, _maxBullets);
        }

        public GameObject RequestBullet()
        {
            GameObject bullet = _bulletPool.Get();
            bullet.GetComponent<BulletComponent>().Initialize();
            return bullet;
        }

        public void ReturnBullet(GameObject bullet)
        {
            _bulletPool.Release(bullet);
        }

        GameObject CreateBullet()
        {
            var bulletObj = Instantiate(_bulletPrefab);
            bulletObj.transform.SetParent(transform, true);
            bulletObj.GetComponent<BulletComponent>().Initialize();
            return bulletObj;
        }

        void OnBulletReturnedToPool(GameObject bullet)
        {
            bullet.SetActive(false);
        }

        void OnTakeBulletFromPool(GameObject bullet)
        {
            bullet.SetActive(true);
        }
    }
}
