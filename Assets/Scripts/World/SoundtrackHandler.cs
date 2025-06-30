using Assets.Scripts.World;
using System.Collections;
using UnityEngine;

public class SoundtrackHandler : MonoBehaviour
{
    [SerializeField] AudioSource _futureSoundTrack;
    [SerializeField] AudioSource _medievalSoundTrack;
    private float _startVolumeFuture = 0f;
    private float _startVolumeMedieval = 0f;
    private const float TimeToFade = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startVolumeFuture = _futureSoundTrack.volume;
        _startVolumeMedieval = _medievalSoundTrack.volume;
        _medievalSoundTrack.volume = 0f;
        _futureSoundTrack.Play();
        _medievalSoundTrack.Play();

        WorldSwapHandler.Instance.OnWorldSwap.AddListener(SwitchSoundtrack);
    }

    void SwitchSoundtrack()
    {
        if (WorldSwapHandler.Instance.IsInCyberpunkWorld)
        {
            StartCoroutine(FadeTrack(_futureSoundTrack, _startVolumeFuture, _medievalSoundTrack));
        }
        else
        {
            StartCoroutine(FadeTrack(_medievalSoundTrack, _startVolumeMedieval, _futureSoundTrack));
        }
    }

    private IEnumerator FadeTrack(AudioSource fadeInTrack, float volumeFadeIn, AudioSource fadeOutTrack)
    {
        float timeElapsed = 0f;
        float volumeFadeOut = fadeOutTrack.volume;

        while (timeElapsed < TimeToFade)
        {
            fadeInTrack.volume = Mathf.Lerp(0, volumeFadeIn, timeElapsed / TimeToFade);
            fadeOutTrack.volume = Mathf.Lerp(volumeFadeOut, 0, timeElapsed / TimeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        fadeInTrack.volume = volumeFadeIn;
        fadeOutTrack.volume = 0;

    }
}
