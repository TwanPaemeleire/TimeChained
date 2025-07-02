using Assets.Scripts.Boss;
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

            for (int idx = 0; idx < _amountOfDirections; ++idx)
            {
                var bulletObj = BulletsHandler.Instance.RequestBullet(BulletType.Boss);
                bulletObj.transform.position = transform.position;
                bulletObj.GetComponent<PulseBullet>().SetShooterTag(transform.tag);

                Vector2 direction = new Vector2(Mathf.Cos(_directionAngle * Mathf.Deg2Rad), Mathf.Sin(_directionAngle * Mathf.Deg2Rad));

                bulletObj.transform.right = direction.normalized;
                _directionAngle += _angleStep;
                if (_spawnAllAtOnce) yield return null;
                else yield return new WaitForSeconds(_bulletDelay);
            }
            OnAttackFinished?.Invoke();
        }
    }
}