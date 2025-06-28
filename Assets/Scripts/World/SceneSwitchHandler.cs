using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.World
{
    public class SceneSwitchHandler : MonoBehaviour
    {
        [SerializeField] private Animator _transitionAnimator;
        [SerializeField] private float _transitionDuration = 1.0f;
        [SerializeField] private bool _hasBeginAnimation = true;

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
            _transitionAnimator.SetTrigger("Begin");

            yield return new WaitForSeconds(_transitionDuration);

            SceneManager.LoadSceneAsync(sceneId);
        }
    }
}
