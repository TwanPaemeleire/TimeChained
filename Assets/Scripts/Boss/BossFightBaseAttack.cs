using UnityEngine;
using UnityEngine.Events;

public abstract class BossFightBaseAttack : MonoBehaviour
{
    [SerializeField] private bool _canExecuteConsecutive = false;
    public bool CanExecuteConsecutive {  get { return _canExecuteConsecutive; }}
    [SerializeField] private float _delayAfterAttack = 1.0f;
    public float DelayAfterAttack { get { return _delayAfterAttack; }}
    private float _attackSpeedMultiplier = 1.0f;
    public float AttackSpeedMultiplier { set { _attackSpeedMultiplier = value; } get { return _attackSpeedMultiplier; } }

    public UnityEvent OnAttackFinished = new UnityEvent();

    public abstract void Execute();
}
