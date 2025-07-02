using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.SharedLogic;
using Assets.Scripts.Boss;
using Assets.Scripts.World;

namespace Assets.Scripts.Boss
{
    public class BossBehavior : MonoBehaviour
    {
        [SerializeField] private List<BossFightBaseAttack> _attacks;
        [SerializeField] private float _firstAttackDelay = 1.0f;
        [SerializeField] private AnimatorOverrideController _pastControllerOverride;
        [SerializeField] private AnimatorOverrideController _futureControllerOverride;
        private Animator _animator;
        private BossFightBaseAttack _currentAttack = null;
        private int _lastAttackIndex = -1;
        private float _attackSpeedMultiplier = 1.0f;

        private void Start()
        {
            GetComponent<HealthComponent>().OnDeath.AddListener(OnDeath);
            _animator = GetComponent<Animator>();
            WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
        }

        public void OnPlayerArrivedInArena()
        {
            //Invoke(nameof(DoNewAttack), _firstAttackDelay);
        }

        void DoNewAttack()
        {
            if (_currentAttack != null)
            {
                _currentAttack.OnAttackFinished.RemoveListener(OnCurrentAttackFinished);
            }
            bool newAttackFound = false;
            do
            {
                int randIndex = Random.Range(0, _attacks.Count);
                if (randIndex == _lastAttackIndex && !_currentAttack.CanExecuteConsecutive) continue;
                newAttackFound = true;
                BossFightBaseAttack newAttack = _attacks[randIndex];
                _lastAttackIndex = randIndex;
                _currentAttack = newAttack;
                _currentAttack.OnAttackFinished.AddListener(OnCurrentAttackFinished);
                _currentAttack.AttackSpeedMultiplier = _attackSpeedMultiplier;
                _currentAttack.Execute();

            } while (!newAttackFound);
        }

        void OnCurrentAttackFinished()
        {
            Invoke(nameof(DoNewAttack), _currentAttack.DelayAfterAttack);
        }

        void OnDeath()
        {
            _currentAttack.OnAttackFinished.RemoveListener(OnCurrentAttackFinished);
            CancelInvoke(nameof(DoNewAttack));
        }

        void OnWorldSwap()
        {
            if(WorldSwapHandler.Instance.IsInCyberpunkWorld)
            {
                _animator.runtimeAnimatorController = _futureControllerOverride;
            }
            else
            {
                _animator.runtimeAnimatorController = _pastControllerOverride;
            }
        }
    }
}
