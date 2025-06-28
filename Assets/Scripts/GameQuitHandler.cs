using UnityEngine;

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
