using Assets.Scripts.World;
using UnityEngine;

public class LevelGraphicsSwapHandler : MonoBehaviour
{
    [SerializeField] private GameObject _futureWorldObj;
    [SerializeField] private GameObject _pastWorldObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _futureWorldObj.SetActive(WorldSwapHandler.Instance.IsInCyberpunkWorld);
        _pastWorldObj.SetActive(!WorldSwapHandler.Instance.IsInCyberpunkWorld);

        WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
        WorldSwapHandler.Instance.OnWorldFlicker.AddListener(OnWorldFlicker);
    }

    void OnWorldSwap()
    {
        _futureWorldObj.SetActive(WorldSwapHandler.Instance.IsInCyberpunkWorld);
        _pastWorldObj.SetActive(!WorldSwapHandler.Instance.IsInCyberpunkWorld);
    }

    void OnWorldFlicker()
    {
        _futureWorldObj.SetActive(WorldSwapHandler.Instance.IsFlickeringInCyberpunkWorld);
        _pastWorldObj.SetActive(!WorldSwapHandler.Instance.IsFlickeringInCyberpunkWorld);
    }
}
