using Unity.Cinemachine;
using UnityEngine;

public class BossFightStartHandler : MonoBehaviour
{
    [SerializeField] private CinemachineBrain _cinemachineBrain;
    [SerializeField] private CinemachineCamera _currentCamera;
    [SerializeField] private CinemachineCamera _bossFightCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _bossFightCamera.gameObject.SetActive(true);
        _currentCamera.gameObject.SetActive(true);
        Invoke(nameof(StartBossFight), _cinemachineBrain.DefaultBlend.Time);
    }

    void StartBossFight()
    {
        // Send event to boss to start
    }
}
