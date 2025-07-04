using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.World
{
    public class SceneSwitchHandler : MonoBehaviour
    {
        [SerializeField] private Animator _transitionAnimator;
        [SerializeField] private float _transitionDuration = 1.0f;
        [SerializeField] private float _nearlyDoneThreshHold = 0.05f;
        public float TransitionDuration{ get { return _transitionDuration; } }
        [SerializeField] private bool _hasBeginAnimation = true;

        public UnityEvent OnSceneSwitchBegin = new UnityEvent();
        public UnityEvent OnSceneSwitchNearlyDone = new UnityEvent();

        private void Awake()
        {
            if (!_hasBeginAnimation)
            {
                _transitionAnimator.enabled = false;
            }
        }

        public void RequestSceneTransition(int sceneId)
        {
            if(!_hasBeginAnimation)
            {
                _transitionAnimator.enabled = true;
            }
            StartCoroutine(SwitchScene(sceneId));
        }

        public void RequestSceneTransition(string sceneName)
        {
            RequestSceneTransition(SceneManager.GetSceneByName(sceneName).buildIndex);
        }

        IEnumerator SwitchScene(int sceneId)
        {
            OnSceneSwitchBegin?.Invoke();
            _transitionAnimator.SetTrigger("Begin");

            yield return new WaitForSeconds(_transitionDuration - _nearlyDoneThreshHold);
            OnSceneSwitchNearlyDone?.Invoke();
            yield return new WaitForSeconds(_nearlyDoneThreshHold);

            SceneManager.LoadSceneAsync(sceneId);
        }
    }
}
