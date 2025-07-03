using UnityEngine;

namespace Assets.Scripts.Miscellaneous
{
    public class PuffControllerComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _playerObject;

        private void OnEnable()
        {
            var position = _playerObject.transform.position;
            gameObject.transform.SetPositionAndRotation(position, Quaternion.identity);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
