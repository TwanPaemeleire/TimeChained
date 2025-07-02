using Assets.Scripts.SharedLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HealthDisplayHandler : MonoBehaviour
    {
        [SerializeField] private HealthComponent _healthToDisplay;
        private Image _fillImage;
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
            _fillImage.fillAmount = _healthToDisplay.GetRemainingHealthPercentage();
        }
    }
}
