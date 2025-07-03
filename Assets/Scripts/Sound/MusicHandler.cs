using Assets.Scripts.World;
using System.Collections;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    // Time left -> could make it so that in the boss fight, the music is not lined up with the world swap
    [Header("Music Settings")]
    [SerializeField] private AudioClip _pastMusic;
    [SerializeField] private AudioClip _futureMusic;
    [SerializeField] private bool _playOnStart = true;
    [SerializeField] private bool _changesOnWorldSwap = true;
    [Header("Fade Settings")]
    [SerializeField] private bool _fadeInOnStart = true;
    [SerializeField] private float _fadeInTargetVolume = 1.0f;
    [SerializeField] private float _volumeFadeInDuration = 0.5f;
    [SerializeField] private SceneSwitchHandler _sceneSwitchHandler;
    [SerializeField] private bool _fadeOutWithSceneTransition = true;
    [SerializeField] private float _volumeFadeOutDuration = 0.5f;

    private AudioSource _audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_changesOnWorldSwap)
        {
            OnWorldSwap();
            WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
        }
        else
        {
            if(_audioSource.clip == null)
            {
                if (_futureMusic != null) _audioSource.clip = _futureMusic;
                else if (_pastMusic != null) _audioSource.clip = _pastMusic;
            }
        }
        if (_fadeOutWithSceneTransition)
        {
            _sceneSwitchHandler.OnSceneSwitchBegin.AddListener(FadeOutMusic);
            _volumeFadeOutDuration = _sceneSwitchHandler.TransitionDuration;
        }
        if (_fadeInOnStart)
        {
            _audioSource.volume = 0.0f;
            StartCoroutine(GraduallyChangeVolume(_fadeInTargetVolume, _volumeFadeInDuration));
        }
    }

    void OnWorldSwap()
    {
        float currentTime = (_audioSource.clip != null) ? _audioSource.time : 0.0f;
        if (WorldSwapHandler.Instance.IsInCyberpunkWorld)
        { 
         _audioSource.clip = _futureMusic;
        }
        else
        {
            _audioSource.clip = _pastMusic;
        }
        _audioSource.time = currentTime;
        _audioSource.Play();
    }

    public void PlayAndFadeInFromStart()
    {
        StartCoroutine(GraduallyChangeVolume(_fadeInTargetVolume, _volumeFadeInDuration));
        OnWorldSwap();
    }

    public void FadeOutMusic()
    {
        StartCoroutine(GraduallyChangeVolume(0.0f, _volumeFadeOutDuration));
    }

    IEnumerator GraduallyChangeVolume(float targetVolume, float fadeDuration)
    {
        float progress = 0.0f;
        float startVolume = _audioSource.volume;
        while (progress < fadeDuration)
        {
            progress += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, targetVolume, progress / fadeDuration);
            yield return null;
        }
        _audioSource.volume = targetVolume;
    }
}
