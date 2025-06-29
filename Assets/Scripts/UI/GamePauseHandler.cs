using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.UI
{
    public class GamePauseHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = _player.GetComponent<PlayerInput>();
        }

        public void UnpauseGame()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            _playerInput.enabled = true;
        }

        public void PauseGame(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Time.timeScale = 0f;
                gameObject.SetActive(true);
                _playerInput.enabled = false;
            }
        }
    }
}

