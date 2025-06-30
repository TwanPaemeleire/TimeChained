using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyMovementComponent : MonoBehaviour
    {
        [SerializeField] private List<Transform> _patrolPoints = new List<Transform>();
        [SerializeField] private float _movementSpeed = 4.0f;

        private int _currentTargetPointIndex = 0;
        private Vector3 _direction = Vector3.zero;

        private EnemyAIComponent _enemyAIComponent;
        private SpriteRenderer _spriteRenderer;
        private EnemyWeaponRotateComponent _weaponRotateComponent;

        private void Start()
        {
            _enemyAIComponent = GetComponent<EnemyAIComponent>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _weaponRotateComponent = GetComponentInChildren<EnemyWeaponRotateComponent>();

            BeginMovement();
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

                _enemyAIComponent.OnMovementEnd.Invoke();
                StartCoroutine(StandGuard());
            }
        }

        private IEnumerator StandGuard()
        {
            yield return new WaitForSeconds(1.5f);

            BeginMovement();
        }

        private void BeginMovement()
        {
            _direction = (_patrolPoints[_currentTargetPointIndex].position - transform.position).normalized;
            _direction.y = 0;

            if (_direction.x > 0.0f)
            {
                _spriteRenderer.flipX = false;
                _weaponRotateComponent.FlipRight();
            }
            else
            {
                _spriteRenderer.flipX = true;
                _weaponRotateComponent.FlipLeft();
            }

            _enemyAIComponent.OnMovementBegin.Invoke();
        }
    }
}
