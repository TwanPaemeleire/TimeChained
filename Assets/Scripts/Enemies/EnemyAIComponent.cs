using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyAIComponent : MonoBehaviour
    {
        private GameObject _player;
        private int _playerLayer;
        private int _visibilityMask;

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
                }
                else
                {
                    CurrentEnemyState = EnemyState.Patrolling;
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
            }
        }

        private bool IsPlayerVisible()
        {
            Vector2 origin = transform.position;
            Vector2 direction = (_player.transform.position - transform.position).normalized;
            float distance = Vector2.Distance(origin, _player.transform.position);

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, _visibilityMask);

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
