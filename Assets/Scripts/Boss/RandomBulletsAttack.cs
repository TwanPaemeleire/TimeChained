using Assets.Scripts.Boss;
using Assets.Scripts.SharedLogic;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Boss
{
    public class RandomBulletsAttack : BossFightBaseAttack
    {
        [SerializeField] private float _attackDuration = 3.0f;
        [SerializeField] private float _bulletDelay = 0.2f;

        public override void Execute()
        {
            StartCoroutine(ShootOutRandomBullets());
        }

        IEnumerator ShootOutRandomBullets()
        {
            float elapsedTime = 0.0f;
            while(elapsedTime < _attackDuration)
            {
                elapsedTime += _bulletDelay;
                float randomAngle = Random.Range(0f, 360f);
                Vector2 direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
                int randomEnumValue = Random.Range(1, 3);
                BulletType bulletType = (BulletType)randomEnumValue;
                var bulletObj = BulletsHandler.Instance.RequestBullet(bulletType);
                bulletObj.transform.position = BulletSpawnPosition.position;
                bulletObj.transform.right = direction.normalized;
                bulletObj.GetComponent<BulletComponent>().SetShooterTag(transform.tag);
                yield return new WaitForSeconds(_bulletDelay);
            }
            OnAttackFinished?.Invoke();
        }
    }

}
