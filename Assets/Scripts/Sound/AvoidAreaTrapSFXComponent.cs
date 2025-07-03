using Assets.Scripts.World;
using System.Collections;
using UnityEngine;

public class AvoidAreaTrapSFXComponent : MonoBehaviour
{
    private AudioSource _trapSFX;
    private const float TimeToFade = 0.2f;
    private float _startVolume;

    private void Start()
    {
        _trapSFX = GetComponent<AudioSource>();
        _trapSFX.Play();
        _startVolume = _trapSFX.volume;
        WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
    }

    private void OnWorldSwap()
    {
        if (WorldSwapHandler.Instance.IsInCyberpunkWorld)
        {
            StartCoroutine(FadeTrack(_trapSFX, _startVolume));
        }
        else
        {
            StartCoroutine(FadeTrack(_trapSFX, 0));
        }
        
    }

    private IEnumerator FadeTrack(AudioSource fadeInTrack, float volumeFadeIn)
    {
        float timeElapsed = 0f;

        while (timeElapsed < TimeToFade)
        {
            fadeInTrack.volume = Mathf.Lerp(fadeInTrack.volume, volumeFadeIn, timeElapsed / TimeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        fadeInTrack.volume = volumeFadeIn;
    }
}
