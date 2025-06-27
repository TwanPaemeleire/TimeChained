using UnityEngine;
using UnityEngine.UI;

public class HealthDisplayHandler : MonoBehaviour
{
    [SerializeField] private HealthComponent _playerHealth;
    private Slider _healthSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _healthSlider = GetComponent<Slider>();
        UpdateHealthDisplay();
        _playerHealth.OnHit.AddListener(UpdateHealthDisplay);
        _playerHealth.OnHit.AddListener(UpdateHealthDisplay);
    }

    void UpdateHealthDisplay()
    {
        _healthSlider.value = _playerHealth.GetRemainingHealthPercentage();
    }
}
