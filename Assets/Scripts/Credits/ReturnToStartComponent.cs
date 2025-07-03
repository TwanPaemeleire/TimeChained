using Assets.Scripts.World;
using UnityEngine;

public class ReturnToStartComponent : MonoBehaviour
{
    [SerializeField] private SceneSwitchHandler _sceneSwitchHandler;
    private int _newSceneIndex = 0;

    public void Return()
    {
        _sceneSwitchHandler.RequestSceneTransition(_newSceneIndex);
    }
}
