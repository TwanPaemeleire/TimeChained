using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ButtonVisualInteraction : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            //_button.On
        }
    }
}
