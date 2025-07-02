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
        BossDefault,
        BossPulse,
    }

    public class BulletsHandler : MonoSingleton<BulletsHandler>
    {
        [SerializeField] private int _maxPlayerBullets = 10;
        [SerializeField] private int _maxBossPulseBullets = 10;
        [SerializeField] private int _maxBossDefaultBullets = 10;
        [SerializeField] private GameObject _playerBulletPrefab;
        [SerializeField] private GameObject _bossDefaultBulletPrefab;
        [SerializeField] private GameObject _bossPulseBulletPrefab;
        private Dictionary<BulletType, ObjectPool<GameObject>> _bulletPools = new Dictionary<BulletType, ObjectPool<GameObject>>();

        private void Awake()
        {
            _bulletPools.Add(BulletType.Player, new ObjectPool<GameObject>(CreatePlayerBullet, OnTakeBulletFromPool, OnBulletReturnedToPool, null, true, _maxPlayerBullets, _maxPlayerBullets));
            _bulletPools.Add(BulletType.BossDefault, new ObjectPool<GameObject>(CreateBossDefaultBullet, OnTakeBulletFromPool, OnBulletReturnedToPool, null, true, _maxBossDefaultBullets, _maxBossDefaultBullets));
            _bulletPools.Add(BulletType.BossPulse, new ObjectPool<GameObject>(CreateBossPulseBullet, OnTakeBulletFromPool, OnBulletReturnedToPool, null, true, _maxBossPulseBullets, _maxBossPulseBullets));
        }

        public GameObject RequestBullet(BulletType type)
        {
            GameObject bullet = _bulletPools[type].Get();
            if (type == BulletType.Player || type == BulletType.BossDefault)
            {
                bullet.GetComponent<BulletComponent>().Initialize();
            }
            else if(type == BulletType.BossPulse)
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

        GameObject CreateBossDefaultBullet()
        {
            var bulletObj = Instantiate(_bossDefaultBulletPrefab);
            bulletObj.transform.SetParent(transform, true);
            bulletObj.GetComponent<BulletComponent>().BulletType = BulletType.BossDefault;
            return bulletObj;
        }

        GameObject CreateBossPulseBullet()
        {
            var bulletObj = Instantiate(_bossPulseBulletPrefab);
            bulletObj.transform.SetParent(transform, true);
            PulseBullet pulseBullet = bulletObj.GetComponent<PulseBullet>();
            pulseBullet.BulletType = BulletType.BossPulse;
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
