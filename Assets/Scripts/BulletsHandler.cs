using UnityEngine;

public class BulletsHandler : MonoSingleton<BulletsHandler>
{
    [SerializeField] private int _maxBullets = 10;
    [SerializeField] private GameObject _bulletPrefab;

    private GameObject[] _bullets;
    private float[] _bulletsActiveTime;

    private void Awake()
    {
        _bullets = new GameObject[_maxBullets]; 
        _bulletsActiveTime = new float[_maxBullets];
        for (int i = 0; i < _maxBullets; i++)
        {
            _bullets[i] = Instantiate(_bulletPrefab);
            _bullets[i].SetActive(false);
        }
    }


    public GameObject RequestBullet()
    {
        int oldestIndex = -1;
        float oldestTime = float.MaxValue;

        for (int i = 0; i < _bullets.Length; i++)
        {
            if (!_bullets[i].activeSelf)
            {
                _bullets[i].gameObject.SetActive(true);
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

        _bullets[oldestIndex].gameObject.SetActive(true);
        _bulletsActiveTime[oldestIndex] = Time.time;
        return _bullets[oldestIndex];
    }
}
