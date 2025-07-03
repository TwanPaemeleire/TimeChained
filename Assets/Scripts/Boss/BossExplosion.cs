using UnityEngine;
using UnityEngine.Events;

public class BossExplosion : MonoBehaviour
{
    private Animator _animator;
    public UnityEvent<GameObject> OnFinishedPlaying = new UnityEvent<GameObject>();
    public UnityEvent OnStartedPlaying = new UnityEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        OnStartedPlaying?.Invoke();
    }

    // Update is called once per frame
    public void OnAnimationFinished()
    {
        OnFinishedPlaying?.Invoke(this.gameObject);
    }
}
