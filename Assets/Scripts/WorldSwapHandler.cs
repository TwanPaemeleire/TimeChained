using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WorldSwapHandler : MonoSingleton<WorldSwapHandler>
{
    [SerializeField] private float _timeBetweenSwaps = 5.0f;

    public UnityEvent OnWorldSwap = new UnityEvent();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WorldSwapCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WorldSwapCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(_timeBetweenSwaps);
            OnWorldSwap?.Invoke();
        }   
    }
}
