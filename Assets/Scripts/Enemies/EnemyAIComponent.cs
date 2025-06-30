using Assets.Scripts.SharedLogic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Enemies
{
    public class EnemyAIComponent : MonoBehaviour
    {
        [SerializeField] private ShootComponent _enemyWeaponComponent;

        public UnityEvent OnMovementBegin = new UnityEvent();
        public UnityEvent OnMovementEnd = new UnityEvent();

        public UnityEvent OnAttackingBegin = new UnityEvent();
        public UnityEvent OnAttackingEnd = new UnityEvent();

        private GameObject _player;
        private int _playerLayer;
        private int _visibilityMask;

        public Vector3 PlayerDirection { get; private set; }

        public enum EnemyState
        {
            Dormant,
            Patrolling,
            Attacking
        }

        public EnemyState CurrentEnemyState { get; private set; } = EnemyState.Dormant;

        private void Start()
        {
            _playerLayer = LayerMask.NameToLayer("Player");
            _player = GameObject.FindWithTag("Player");
            _visibilityMask = LayerMask.GetMask("Player", "Ground");
        }

        private void Update()
        {
            if (CurrentEnemyState != EnemyState.Dormant)
            {
                if (IsPlayerVisible())
                {
                    CurrentEnemyState = EnemyState.Attacking;
                    OnAttackingBegin.Invoke();
                    _enemyWeaponComponent.SetShouldShoot(true);
                    _enemyWeaponComponent.SetDirection(PlayerDirection);
                }
                else
                {
                    CurrentEnemyState = EnemyState.Patrolling;
                    OnAttackingEnd.Invoke();
                    _enemyWeaponComponent.SetShouldShoot(false);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == _playerLayer)
            {
                if (CurrentEnemyState == EnemyState.Dormant)
                {
                    CurrentEnemyState = EnemyState.Patrolling;
                    Debug.Log("Player spotted. Patrolling started");
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.layer == _playerLayer)
            {
                CurrentEnemyState = EnemyState.Dormant;
                _enemyWeaponComponent.SetShouldShoot(false);
            }
        }

        private bool IsPlayerVisible()
        {
            Vector2 origin = transform.position;
            PlayerDirection = (_player.transform.position - transform.position).normalized;
            float distance = Vector2.Distance(origin, _player.transform.position);

            RaycastHit2D hit = Physics2D.Raycast(origin, PlayerDirection, distance, _visibilityMask);

            if(!hit.collider)
            {
                return false;
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                return false;
            }

            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }

            return false;
        }
    }
}
