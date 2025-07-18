using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.World
{
    public class WorldSwapHandler : MonoSingleton<WorldSwapHandler>
    {
        private float _timeBetweenSwaps = 10.0f;
        [SerializeField] private int _amountOfFlickers = 4;
        [SerializeField] private float _flickerDuration = 0.2f;

        private bool _isInCyberpunkWorld = true;
        public bool IsInCyberpunkWorld { get { return _isInCyberpunkWorld; } }

        private bool _isFlickeringInCyberpunkWorld = false;
        public bool IsFlickeringInCyberpunkWorld { get { return _isFlickeringInCyberpunkWorld; } }

        public UnityEvent OnWorldSwap = new UnityEvent();
        public UnityEvent OnFinalFlicker = new UnityEvent();
        public UnityEvent OnNewWorldFlicker = new UnityEvent();
        public UnityEvent OnCurrentWorldBackFlicker = new UnityEvent();
        public UnityEvent OnWorldFlicker = new UnityEvent();


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartCoroutine(WorldSwapCoroutine());
        }

        IEnumerator WorldSwapCoroutine()
        {
            while(true)
            {
                yield return new WaitForSeconds(_timeBetweenSwaps - (_amountOfFlickers * 2 * _flickerDuration));

                for(int flickerCounter = 0; flickerCounter < _amountOfFlickers; ++flickerCounter)
                {
                    if (flickerCounter == _amountOfFlickers - 1) OnFinalFlicker?.Invoke();

                    OnWorldFlicker?.Invoke();
                    _isFlickeringInCyberpunkWorld = !_isFlickeringInCyberpunkWorld;
                    yield return new WaitForSeconds(_flickerDuration);
                    OnWorldFlicker?.Invoke();
                    _isFlickeringInCyberpunkWorld = !_isFlickeringInCyberpunkWorld;
                    yield return new WaitForSeconds(_flickerDuration);

                    //OnNewWorldFlicker?.Invoke();
                    //yield return new WaitForSeconds(_flickerDuration);
                    //OnCurrentWorldBackFlicker?.Invoke();
                    //yield return new WaitForSeconds(_flickerDuration);
                }

                _isInCyberpunkWorld = !_isInCyberpunkWorld;
                OnWorldSwap?.Invoke();
            }   
        }
    }
}
