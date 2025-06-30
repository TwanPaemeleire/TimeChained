using UnityEngine;

public abstract class BossFightBaseAttack : MonoBehaviour
{
    [SerializeField] private bool _canExecuteConsecutive = false;

    public abstract void Execute();
}
