using System;
using System.Collections.Generic;
using Assets.Scripts.Boss;
using Assets.Scripts.World;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.SharedLogic
{
    public enum BulletType
    {
        Player,
        Boss,
    }

    public class BulletsHandler : MonoSingleton<BulletsHandler>
    {
        [SerializeField] private int _maxPlayerBullets = 10;
        [SerializeField] private int _maxBossBullets = 10;
        [SerializeField] private GameObject _playerBulletPrefab;
        [SerializeField] private GameObject _bossBulletPrefab;
        private Dictionary<BulletType, ObjectPool<GameObject>> _bulletPools = new Dictionary<BulletType, ObjectPool<GameObject>>();

        private void Awake()
        {
            _bulletPools.Add(BulletType.Player, new ObjectPool<GameObject>(CreatePlayerBullet, OnTakeBulletFromPool, OnBulletReturnedToPool, null, true, _maxPlayerBullets, _maxPlayerBullets));
            _bulletPools.Add(BulletType.Boss, new ObjectPool<GameObject>(CreateBossBullet, OnTakeBulletFromPool, OnBulletReturnedToPool, null, true, _maxBossBullets, _maxBossBullets));
        }

        public GameObject RequestBullet(BulletType type)
        {
            GameObject bullet = _bulletPools[type].Get();
            if(type == BulletType.Player)
            {
                bullet.GetComponent<BulletComponent>().Initialize();
            }
            else if(type == BulletType.Boss)
            {
                bullet.GetComponent<PulseBullet>().Initialize();
            }
            return bullet;
        }

        public void ReturnBullet(BulletType type, GameObject bullet)
        {
            _bulletPools[type].Release(bullet);
        }

        GameObject CreatePlayerBullet()
        {
            var bulletObj = Instantiate(_playerBulletPrefab);
            bulletObj.transform.SetParent(transform, true);
            return bulletObj;
        }

        GameObject CreateBossBullet()
        {
            var bulletObj = Instantiate(_bossBulletPrefab);
            bulletObj.transform.SetParent(transform, true);
            PulseBullet pulseBullet = bulletObj.GetComponent<PulseBullet>();
            pulseBullet.BulletType = BulletType.Boss;
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
