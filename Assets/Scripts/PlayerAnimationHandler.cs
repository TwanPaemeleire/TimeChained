using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private AnimatorOverrideController _medievalControllerOverride;
    private Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnWorldSwap()
    {
        _animator.runtimeAnimatorController = _medievalControllerOverride;
    }
}
