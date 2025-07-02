using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Boss
{
    public class BossFightStartHandler : MonoBehaviour
    {
        [SerializeField] private BossBehavior _boss;
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private CinemachineCamera _currentCamera;
        [SerializeField] private CinemachineCamera _bossFightCamera;
        [SerializeField] private PixelPerfectCamera _pixelPerfectCamera;
        [SerializeField] private int _targetAssetsPPU = 64;
        [SerializeField] private BoxCollider2D _playerPushCollider;
        [SerializeField] private Transform _playerPushEndTransform;
        private Vector3 _playerPushEndPos;

        public UnityEvent OnPlayerArrivedInArena = new UnityEvent();

        private void Start()
        {
            _playerPushEndPos = _playerPushEndTransform.position;
            OnPlayerArrivedInArena.AddListener(_boss.OnPlayerArrivedInArena);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                StartCoroutine(HandleBossFightStart(collision.gameObject));
            }
        }

        IEnumerator HandleBossFightStart(GameObject player)
        {
            var playerInput = player.GetComponent<PlayerInput>();
            playerInput.DeactivateInput();

            _bossFightCamera.gameObject.SetActive(true);
            _currentCamera.gameObject.SetActive(false);

            _playerPushCollider.gameObject.SetActive(true);

            Vector3 newBoxPos = player.transform.position;
            newBoxPos.x -= _playerPushCollider.size.x / 2 + _playerPushCollider.size.x;
            _playerPushCollider.transform.position = newBoxPos;

            float elapsedTime = 0.0f;
            Vector3 startPos = _playerPushCollider.transform.position;
            float cinemachineBlendTime = _cinemachineBrain.DefaultBlend.Time;

            int startPPU = _pixelPerfectCamera.assetsPPU;

            while (elapsedTime < cinemachineBlendTime)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / cinemachineBlendTime;
                float newX = Mathf.SmoothStep(startPos.x, _playerPushEndPos.x, progress);
                Vector3 newPos = new Vector3(newX, _playerPushEndPos.y, _playerPushEndPos.z);
                _playerPushCollider.transform.position = newPos;

                int newPPU = Mathf.RoundToInt(Mathf.Lerp(startPPU, _targetAssetsPPU, progress));
                _pixelPerfectCamera.assetsPPU = newPPU;

                yield return null;
            }
            _pixelPerfectCamera.assetsPPU = _targetAssetsPPU;

            playerInput.ActivateInput();
            StartBossFight();
        }

        void StartBossFight()
        {
            // Send event to boss to start
            OnPlayerArrivedInArena?.Invoke();
        }
    }
}
