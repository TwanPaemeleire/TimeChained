using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Assets.Scripts.Boss
{
    public class PulseBullet : Assets.Scripts.SharedLogic.BulletComponent
    {
        [Header("Impulse Settings")]
        [SerializeField] private float _pulseDelay = 1.0f;
        [SerializeField] private float _pulseStrenght = 2.0f;
        [SerializeField] private float _dragStrenght = 1.5f;
        [SerializeField] private float _impulseApplySpeed = 5.0f;
        [SerializeField] private float _minimumSpeed = 5.0f;

        Coroutine _impulseCoroutine;
        Coroutine _dragCoroutine;
        // Change sprites on pulse???

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            InvokeRepeating(nameof(DoImpulse), 0, _pulseDelay);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        protected override void DestroyBullet()
        {
            CancelInvoke(nameof(DoImpulse));
            StopCoroutine(_impulseCoroutine);
            StopCoroutine(_dragCoroutine);
            base.DestroyBullet();
        }

            void DoImpulse()
        {
            _impulseCoroutine = StartCoroutine(ApplyImpulse());
            _dragCoroutine = StartCoroutine(ApplyDrag());
        }

        IEnumerator ApplyImpulse()
        {
            float addedImpulse = 0.0f;
            while(addedImpulse < _pulseStrenght)
            {
                float impulseToAdd = _pulseStrenght * Time.deltaTime * _impulseApplySpeed;
                addedImpulse += impulseToAdd;
                _speed += impulseToAdd;
            }
            yield return null;
        }

        IEnumerator ApplyDrag()
        {
            while(_speed > _minimumSpeed)
            {
                _speed -= _dragStrenght * Time.deltaTime;
                yield return null;
            }
            _speed = _minimumSpeed;
        }
    }
}
