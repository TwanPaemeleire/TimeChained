using Assets.Scripts.Boss;
using Assets.Scripts.SharedLogic;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBulletWallsAttack : BossFightBaseAttack
{
    [SerializeField] private int _amountOfShooters = 4;
    [SerializeField] private float _bulletShootDelay = 0.2f;
    [SerializeField] private float _amountOffFullRotations = 1.0f;
    [SerializeField] private float _rotationSpeed = 1.0f;

    private float _shooterAngleInterval;
    List<Coroutine> _shootingCoroutines = new List<Coroutine>();
    List<float> _shootingAngles = new List<float>();
    public override void Execute()
    {
        _shooterAngleInterval = 360.0f / (float)_amountOfShooters;
        for(int shooterIdx =0; shooterIdx < _amountOfShooters; ++shooterIdx)
        {
            float angle = shooterIdx * _shooterAngleInterval;
            _shootingAngles.Add(angle);
            _shootingCoroutines.Add(StartCoroutine(ShootWhileRotating(angle, shooterIdx)));
        }
    }

    IEnumerator ShootWhileRotating(float startAngle, int shooterIdx)
    {
        float angleTracker = 0.0f;
        _shootingAngles[shooterIdx] = startAngle;

        StartCoroutine(ShootBullet(shooterIdx));

        while (angleTracker < _amountOffFullRotations * 360.0f)
        {
            float angleToAdd = _rotationSpeed * Time.deltaTime;
            _shootingAngles[shooterIdx] += angleToAdd;
            angleTracker += angleToAdd;
            yield return null;
        }

        _shootingAngles.Clear();
        OnAttackFinished?.Invoke();
        StopAllCoroutines();
    }

    IEnumerator ShootBullet(int shooterIdx)
    {
        while(true)
        {
            Vector2 direction = new Vector2(Mathf.Cos(_shootingAngles[shooterIdx] * Mathf.Deg2Rad), Mathf.Sin(_shootingAngles[shooterIdx] * Mathf.Deg2Rad));

            var bulletObj = BulletsHandler.Instance.RequestBullet(BulletType.BossDefault);
            bulletObj.transform.position = transform.position;
            bulletObj.transform.right = direction.normalized;
            bulletObj.GetComponent<BulletComponent>().SetShooterTag(transform.tag);
            yield return new WaitForSeconds(_bulletShootDelay);
        }
    }
}
