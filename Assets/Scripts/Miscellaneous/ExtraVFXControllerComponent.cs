using UnityEngine;

namespace Assets.Scripts.Miscellaneous
{
    public class ExtraVFXControllerComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _playerObject;
        [SerializeField] private Vector3 _offset;

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
