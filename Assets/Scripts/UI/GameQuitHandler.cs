using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GameQuitHandler : MonoBehaviour
    {
        public void RequestQuit()
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
