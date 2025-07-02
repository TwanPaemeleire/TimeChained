using UnityEngine;

namespace Assets.Scripts.Miscellaneous
{
    public class RandomSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _audioClips;
        private AudioSource _audioSource;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayRandomSound()
        {
            if(_audioClips.Length == 0)
            {
                Debug.LogWarning("RandomSoundPlayer has no clips assigned in inspector");
                return;
            }
            int randomIndex = Random.Range(0, _audioClips.Length);
            _audioSource.clip = _audioClips[randomIndex];
            _audioSource.Play();
        }
    }
}