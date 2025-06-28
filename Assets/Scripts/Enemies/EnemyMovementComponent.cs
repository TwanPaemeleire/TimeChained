using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemies;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyMovementComponent : MonoBehaviour
    {
        [SerializeField] private List<Transform> _patrolPoints = new List<Transform>();
        [SerializeField] private float _movementSpeed = 4.0f;

        private int _currentTargetPointIndex = 0;
        private Vector3 _direction = Vector3.zero;

        private EnemyAIComponent _enemyAIComponent;


        private void Start()
        {
            _enemyAIComponent = GetComponent<EnemyAIComponent>();

            _direction = (_patrolPoints[_currentTargetPointIndex].position - transform.position).normalized;
            _direction.y = 0;
        }

        private void Update()
        {
            if (_enemyAIComponent.CurrentEnemyState == EnemyAIComponent.EnemyState.Dormant 
                || _patrolPoints == null || _patrolPoints.Count == 0 )
            {
                return;
            }

            if (_direction == Vector3.zero)
            {
                return;
            }

            var speed = _movementSpeed;

            if (_enemyAIComponent.CurrentEnemyState == EnemyAIComponent.EnemyState.Attacking)
            {
                speed *= 0.5f;
            }

            transform.position += speed * Time.deltaTime * _direction;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("PatrolPoint"))
            {
                _direction = Vector3.zero;
                _currentTargetPointIndex++;

                if (_currentTargetPointIndex == _patrolPoints.Count)
                {
                    _currentTargetPointIndex = 1;
                    _patrolPoints.Reverse();
                }

                StartCoroutine(StandGuard());
            }
        }

        private IEnumerator StandGuard()
        {
            yield return new WaitForSeconds(1.5f);

            _direction = (_patrolPoints[_currentTargetPointIndex].position - transform.position).normalized;
            _direction.y = 0;
        }
    }
}
