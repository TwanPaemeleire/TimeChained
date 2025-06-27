using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ShootComponent : MonoBehaviour
{
    [SerializeField] private float _fireRate;
    [SerializeField] private bool _shouldShoot; //for eg traps that just always shoot
    [SerializeField] private float _offset = 0.6f;
    [SerializeField] private Vector3 _direction = new Vector3(1, 0, 0);
    private float _accumulatedTime;

    public UnityEvent OnShoot = new UnityEvent();

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
        GameObject bullet = BulletsHandler.Instance.RequestBullet();
        if (bullet != null)
        {
            bullet.transform.position = transform.position + _direction.normalized * _offset;
            bullet.transform.right = _direction.normalized;
            OnShoot?.Invoke();
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
}
