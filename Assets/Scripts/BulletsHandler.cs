using System;
using UnityEngine;

public class BulletsHandler : MonoSingleton<BulletsHandler>
{
    [SerializeField] private int _maxBullets = 10;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Sprite _medievalBulletSprite;

    private Sprite _cyberBulletSprite;
    private Tuple<GameObject, BulletComponent>[] _bullets;
    private SpriteRenderer[] _bulletRenderers;
    private float[] _bulletsActiveTime;

    private void Start()
    {
        _bullets = new Tuple<GameObject, BulletComponent>[_maxBullets]; 
        _bulletsActiveTime = new float[_maxBullets];
        _bulletRenderers = new SpriteRenderer[_maxBullets];
        for (int i = 0; i < _maxBullets; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab);
            _bullets[i] = new Tuple<GameObject, BulletComponent>(bullet, bullet.GetComponent<BulletComponent>());
            _bulletRenderers[i] = _bullets[i].Item1.GetComponent<SpriteRenderer>();
            _bullets[i].Item1.SetActive(false);
        }
        _cyberBulletSprite = _bulletRenderers[0].sprite;
        WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
    }


    public Tuple<GameObject, BulletComponent> RequestBullet()
    {
        int oldestIndex = -1;
        float oldestTime = float.MaxValue;

        for (int i = 0; i < _bullets.Length; i++)
        {
            if (!_bullets[i].Item1.activeSelf)
            {
                _bullets[i].Item1.SetActive(true);
                _bulletsActiveTime[i] = Time.time;
                return _bullets[i];
            }

            else //object is active (for when memory pool is all active)
            {
                if (_bulletsActiveTime[i] < oldestTime)
                {
                    oldestIndex = i;
                    oldestTime = _bulletsActiveTime[i];
                }
            }
        }

        _bullets[oldestIndex].Item1.SetActive(true);
        _bulletsActiveTime[oldestIndex] = Time.time;
        return _bullets[oldestIndex];
    }

    void OnWorldSwap()
    {
        if (WorldSwapHandler.Instance.IsInCyberpunkWorld)
        {
            foreach (SpriteRenderer bulletSpriteRenderer in _bulletRenderers)
            {
                bulletSpriteRenderer.sprite = _cyberBulletSprite;
            }
        }
        else
        {
            foreach (SpriteRenderer bulletSpriteRenderer in _bulletRenderers)
            {
                bulletSpriteRenderer.sprite = _medievalBulletSprite;
            }
        }
    }
}
