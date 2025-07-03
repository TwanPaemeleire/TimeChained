using Assets.Scripts.SharedLogic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HealthDisplayHandler : MonoBehaviour
    {
        [SerializeField] private HealthComponent _healthToDisplay;
        [SerializeField] private float _timeToApplyHealthChange = 0.5f;
        private Image _fillImage;
        private float _targetFillAmount;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _fillImage = GetComponent<Image>();
            UpdateHealthDisplay();
            _healthToDisplay.OnHit.AddListener(UpdateHealthDisplay);
            _healthToDisplay.OnHeal.AddListener(UpdateHealthDisplay);
        }

        void UpdateHealthDisplay()
        {
            _targetFillAmount  = _healthToDisplay.GetRemainingHealthPercentage();
            StartCoroutine(UpdateHealthFill());
        }

        IEnumerator UpdateHealthFill()
        {
            float elapsedTime = 0f;
            float startFillAmount = _fillImage.fillAmount;
            while (elapsedTime < _timeToApplyHealthChange)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / _timeToApplyHealthChange;
                _fillImage.fillAmount = Mathf.SmoothStep(startFillAmount, _targetFillAmount, progress);
                yield return null;
            }
            _fillImage.fillAmount = _targetFillAmount;
        }
    }
}
