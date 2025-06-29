using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ButtonVisualInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Vector3 _maxScale;
        [SerializeField] private float _timeToReachTargetScale = 1.0f;

        private Coroutine _scaleCoroutine;

        public void OnPointerEnter(PointerEventData eventData)
        {
            StartScaleCoroutine(_maxScale);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StartScaleCoroutine(Vector3.one);
        }

        void StartScaleCoroutine(Vector3 targetScale)
        {
            if(_scaleCoroutine !=null)
            {
                StopCoroutine(_scaleCoroutine);
            }
            _scaleCoroutine = StartCoroutine(ChangeButtonScale(targetScale));
        }

        IEnumerator ChangeButtonScale(Vector3 targetScale)
        {
            Vector3 initialScale = transform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < _timeToReachTargetScale)
            {
                float newScale = Mathf.SmoothStep(initialScale.x, targetScale.x, elapsedTime / _timeToReachTargetScale);
                transform.localScale = new Vector3(newScale, newScale, 1.0f);
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
            transform.localScale = targetScale;
        }
    }
}
