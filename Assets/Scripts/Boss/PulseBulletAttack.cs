using Assets.Scripts.SharedLogic;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Boss
{
    public class PulseBulletAttack : BossFightBaseAttack
    {
        [SerializeField] private int _amountOfDirections = 6;
        [SerializeField] private float _rotationBulletSpawnTime = 1.0f;
        [SerializeField] private bool _spawnAllAtOnce = false;
        [SerializeField] private int _amountOfTimesToRepeat = 1;
        [SerializeField] private float _repeatDelay = 0.5f;
        [SerializeField] private float _minRandomAngleOffset = -15.0f;
        [SerializeField] private float _maxRandomAngleOffset = 15.0f;
        private float _bulletDelay;
        private float _angleStep;
        private float _directionAngle = 0.0f;

        public override void Execute()
        {
            StartCoroutine(SpawnProjectiles());
        }

        IEnumerator SpawnProjectiles()
        {
            _directionAngle = 0.0f;
            _bulletDelay = (_rotationBulletSpawnTime / (float)_amountOfDirections) * AttackSpeedMultiplier;
            _angleStep = 360.0f / (float)_amountOfDirections;
            for(int repeatIdx = -1; repeatIdx < _amountOfTimesToRepeat; ++repeatIdx) // Start at -1, because first execution is not considered a repeat
            {
                for (int idx = 0; idx < _amountOfDirections; ++idx)
                {
                    var bulletObj = BulletsHandler.Instance.RequestBullet(BulletType.BossPulse);
                    bulletObj.transform.position = BulletSpawnPosition.position;
                    bulletObj.GetComponent<PulseBullet>().SetShooterTag(transform.tag);

                    Vector2 direction = new Vector2(Mathf.Cos(_directionAngle * Mathf.Deg2Rad), Mathf.Sin(_directionAngle * Mathf.Deg2Rad));

                    bulletObj.transform.right = direction.normalized;
                    _directionAngle += _angleStep;
                    if (_spawnAllAtOnce) yield return null;
                    else yield return new WaitForSeconds(_bulletDelay);
                }
                _directionAngle = 0.0f;
                _directionAngle += Random.Range(_minRandomAngleOffset, _maxRandomAngleOffset);
                yield return new WaitForSeconds(_repeatDelay);
            }
            OnAttackFinished?.Invoke();
        }
    }
}