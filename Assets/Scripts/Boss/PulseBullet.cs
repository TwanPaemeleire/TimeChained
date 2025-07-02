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

        private bool _isFirstImpulse = true;
        Coroutine _impulseCoroutine;
        Coroutine _dragCoroutine;
        // Change sprites on pulse???

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        protected override void DestroyBullet()
        {
            CancelInvoke(nameof(DoImpulse));
            if(_impulseCoroutine != null) StopCoroutine(_impulseCoroutine);
            StopCoroutine(_dragCoroutine);
            base.DestroyBullet();
        }

        public override void Initialize()
        {
            base.Initialize();
            _speed = 0.0f;
            _isFirstImpulse = true;
            InvokeRepeating(nameof(DoImpulse), 0, _pulseDelay);
            _dragCoroutine = StartCoroutine(ApplyDrag());
        }

        void DoImpulse()
        {
            _impulseCoroutine = StartCoroutine(ApplyImpulse());
        }

        IEnumerator ApplyImpulse()
        {
            float addedImpulse = 0.0f;
            while(addedImpulse < _pulseStrenght)
            {
                float impulseToAdd = _pulseStrenght * Time.deltaTime * _impulseApplySpeed;
                addedImpulse += impulseToAdd;
                if(addedImpulse > _pulseStrenght)
                {
                    impulseToAdd = _pulseStrenght - (addedImpulse - impulseToAdd);
                }
                _speed += impulseToAdd;
                yield return null;
            }
            _isFirstImpulse = false;
            _impulseCoroutine = null;
        }

        IEnumerator ApplyDrag()
        {
            while(true)
            {
                _speed -= _dragStrenght * Time.deltaTime;
                if (_speed < _minimumSpeed && !_isFirstImpulse)
                {
                    _speed = _minimumSpeed;
                }
                yield return null;
            }
        }
    }
}
