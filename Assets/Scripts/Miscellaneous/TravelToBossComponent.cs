using Assets.Scripts.World;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelToBossComponent : MonoBehaviour
{
    [SerializeField] private SceneSwitchHandler _sceneSwitchHandler;
    [SerializeField] private int _newSceneIndex = 2;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _sceneSwitchHandler.RequestSceneTransition(_newSceneIndex);
        }
    }
}
