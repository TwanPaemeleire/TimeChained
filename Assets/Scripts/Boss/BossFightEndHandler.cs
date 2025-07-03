using Assets.Scripts.SharedLogic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class BossFightEndHandler : MonoBehaviour
{
    [SerializeField] private float _explosionDelay = 0.2f;
    [SerializeField] private int _explosionPoolSize = 10;
    [SerializeField] private float _explosionCycleDuration = 2.0f;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private float _explosionPositionBorderOffset = 0.5f;
    private Vector2 _ExplosionSpawnArea;
    private float _explosionSpawnAreaHalfWidth;
    private float _explosionSpawnAreaHalfHeight;
    private GameObject _explosionContainer;

    private SpriteRenderer _spriteRenderer;
    private ObjectPool<GameObject> _explosionPool;
    private bool _shouldStopCreatingExplosions = false;

    public UnityEvent OnShouldDoLevelTransition = new UnityEvent(); 


    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _explosionContainer = new GameObject("ExplosionContainer");
        GetComponent<HealthComponent>().OnDeath.AddListener(OnBossDeath);
        _ExplosionSpawnArea = GetComponent<SpriteRenderer>().sprite.bounds.size;
        _explosionSpawnAreaHalfWidth = (_ExplosionSpawnArea.x - _explosionPositionBorderOffset) / 2.0f;
        _explosionSpawnAreaHalfHeight = (_ExplosionSpawnArea.y - _explosionPositionBorderOffset) / 2.0f;
        _explosionPool = new ObjectPool<GameObject>(CreateExplosion, OnGetExplosion, OnReleaseExplosion, OnDestroyExplosion, false, _explosionPoolSize, _explosionPoolSize);
    }

    void OnBossDeath()
    {
        GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in playerObjs)
        {
            if (player.TryGetComponent<HealthComponent>(out var healthComp))
            {
                healthComp.CannotDie = true;
                break;
            }
        }

        StartCoroutine(ExplosionCycle());
        StartCoroutine(TransitionAfterDelay());
    }

    IEnumerator ExplosionCycle()
    {
        while (!_shouldStopCreatingExplosions)
        {
            float randomX = Random.Range(-_explosionSpawnAreaHalfWidth, _explosionSpawnAreaHalfWidth);
            float randomY = Random.Range(-_explosionSpawnAreaHalfHeight, _explosionSpawnAreaHalfHeight);
            Vector2 explosionPosition = transform.position;
            explosionPosition.x += randomX;
            explosionPosition.y += randomY;
            var explosion = _explosionPool.Get();
            explosion.transform.position = explosionPosition;
            yield return new WaitForSeconds(_explosionDelay);
        }
    }

    IEnumerator TransitionAfterDelay()
    {
        yield return new WaitForSeconds(_explosionCycleDuration);
        OnShouldDoLevelTransition?.Invoke();
    }

    public void OnShouldStopSpawningExplosions()
    {
        _shouldStopCreatingExplosions = true;
    }

    private void OnDestroyExplosion(GameObject explosion)
    {
        Destroy(explosion);
    }

    private void OnReleaseExplosion(GameObject explosion)
    {
        explosion.SetActive(false);
    }

    private void OnGetExplosion(GameObject explosion)
    {
        explosion.SetActive(true);
    }

    private GameObject CreateExplosion()
    {
        var explosion = Instantiate(_explosionPrefab);
        explosion.transform.SetParent(_explosionContainer.transform, true);
        explosion.GetComponent<BossExplosion>().OnFinishedPlaying.AddListener((obj) => _explosionPool.Release(obj));
        return explosion;
    }
}
