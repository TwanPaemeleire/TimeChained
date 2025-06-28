using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Miscellaneous
{
    public class LightFlickerComponent : MonoBehaviour
    {
        [SerializeField] private float _flickerInterval = 0.3f;
        [SerializeField] private float _minIntensity = 200.0f;
        [SerializeField] private float _maxIntensity = 200.0f;
        [SerializeField] private float _minOuterRange = 0.8f;
        [SerializeField] private float _maxOuterRange = 1.0f;
        [SerializeField] private float _maxPositionOffset = 0.02f;
        private Light2D _light;
        private Vector3 _startPosition;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _light = GetComponent<Light2D>();
            _startPosition = transform.position;
            InvokeRepeating(nameof(ChangeFlicker), 0, _flickerInterval);
        }

        void ChangeFlicker()
        {
            _light.intensity = Random.Range(_minIntensity, _maxIntensity);
            _light.pointLightOuterRadius = Random.Range(_minOuterRange, _maxOuterRange);

            Vector2 offset = Random.insideUnitCircle * _maxPositionOffset;
            transform.position = _startPosition + new Vector3(offset.x, offset.y, 0f);
        }
    }
}
