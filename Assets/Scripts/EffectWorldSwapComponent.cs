using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectWorldSwapComponent : MonoBehaviour
{
    Volume volume;
    private LensDistortion _lensDistortion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volume = GetComponent<Volume>();
        if(volume.profile.TryGet<LensDistortion>(out LensDistortion lensDistortion))
        {
            _lensDistortion = lensDistortion;
        }

        WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
        WorldSwapHandler.Instance.OnFinalFlicker.AddListener(OnFinalFlicker);
    }

    private void OnFinalFlicker()
    {
        StartCoroutine(LensDistortionIncreaseCoroutine(0.4f, -0.7f));
    }

    private void OnWorldSwap()
    {
        StopCoroutine("LensDistortionIncreaseCoroutine");
        StartCoroutine(LensDistortionDecreaseCoroutine(0.4f, 0f));
    }

    private IEnumerator LensDistortionIncreaseCoroutine(float inTime, float lowIntensity)
    {
        float t = 0f;
        float start = _lensDistortion.intensity.value;
        float exponent = 3f;

        while (t < inTime) //increasing
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / inTime );
            float curve = Mathf.Pow(normalized, exponent); 
            _lensDistortion.intensity.value = Mathf.Lerp(start, lowIntensity, curve);
            yield return null;
        }

        _lensDistortion.intensity.value = lowIntensity;
    }

    private IEnumerator LensDistortionDecreaseCoroutine(float inTime, float intensity)
    {
        float t = 0f;
        float start = _lensDistortion.intensity.value;
        float exponent = 3f;

        while (t < inTime) // decreasing
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / inTime);
            float curve = 1f - Mathf.Pow(1f - normalized, exponent); 
            _lensDistortion.intensity.value = Mathf.Lerp(start, intensity, curve);
            yield return null;
        }

        _lensDistortion.intensity.value = intensity;
    }
}
