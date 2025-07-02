using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Boss
{
    public abstract class BossFightBaseAttack : MonoBehaviour
    {
        [SerializeField] private bool _canExecuteConsecutive = false;
        public bool CanExecuteConsecutive { get { return _canExecuteConsecutive; } }
        [SerializeField] private float _delayAfterAttack = 1.0f;
        public float DelayAfterAttack { get { return _delayAfterAttack; } }
        private float _attackSpeedMultiplier = 1.0f;
        public float AttackSpeedMultiplier { set { _attackSpeedMultiplier = value; } get { return _attackSpeedMultiplier; } }
        [SerializeField] private Transform _bulletSpawnPosition;
        public Transform BulletSpawnPosition { get { return _bulletSpawnPosition; } }

        public UnityEvent OnAttackFinished = new UnityEvent();

        public abstract void Execute();
    }
}
