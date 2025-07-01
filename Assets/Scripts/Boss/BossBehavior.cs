using UnityEngine;
using System.Collections.Generic;

public class BossBehavior : MonoBehaviour
{
    [SerializeField] private List<BossFightBaseAttack> _attacks;
    [SerializeField] private float _firstAttackDelay = 1.0f;
    private BossFightBaseAttack _currentAttack = null;
    private int _lastAttackIndex = -1;
    private float _attackSpeedMultiplier = 1.0f;

    public void OnPlayerArrivedInArena()
    {
        Invoke(nameof(DoNewAttack), _firstAttackDelay);
    }

    void DoNewAttack()
    {
        if (_currentAttack!=null)
        {
            _currentAttack.OnAttackFinished.RemoveListener(OnCurrentAttackFinished);
        }
        bool newAttackFound = false;
        do
        {
            int randIndex = Random.Range(0, _attacks.Count);
            if (randIndex == _lastAttackIndex && !_currentAttack.CanExecuteConsecutive) continue;
            BossFightBaseAttack newAttack = _attacks[randIndex];
            _lastAttackIndex = randIndex;
            _currentAttack = newAttack;
            _currentAttack.OnAttackFinished.AddListener(OnCurrentAttackFinished);
            _currentAttack.Execute();

        } while (!newAttackFound);
    }

    void OnCurrentAttackFinished()
    {
        Invoke(nameof(DoNewAttack), _currentAttack.DelayAfterAttack);
    }
}
