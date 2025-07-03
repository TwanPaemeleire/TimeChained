using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
namespace Assets.Scripts.Credits
{
    public class LotsOfExplosionsComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _explosionPrefab;
        [SerializeField] private int _explosionPoolSize = 10;
        [SerializeField] private float _explosionDelay = 1f;
        [SerializeField] private float _screenWidth;
        [SerializeField] private float _screenHeight;
        private ObjectPool<GameObject> _explosionPool;
        private GameObject _explosionContainer;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _explosionContainer = new GameObject("ExplosionContainer");
            _explosionPool = new ObjectPool<GameObject>(CreateExplosion, OnGetExplosion, OnReleaseExplosion, OnDestroyExplosion, false, _explosionPoolSize, _explosionPoolSize);
            StartCoroutine(nameof(Explosion));
            InvokeRepeating(nameof(Explosion), _explosionDelay, _explosionDelay);
        }

        private void Explosion()
        {
            float randomX = Random.Range(-_screenWidth, _screenWidth);
            float randomY = Random.Range(-_screenHeight, _screenHeight);
            Vector2 explosionPosition = transform.position;
            explosionPosition.x += randomX;
            explosionPosition.y += randomY;
            var explosion = _explosionPool.Get();
            explosion.transform.position = explosionPosition;

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
}


