using UnityEngine;

namespace Code.Scrips.AudioHelpers
{
    public class AudioFader : MonoBehaviour
    {
        private AudioSource _audioSource;
        private const float _FADE_DURATION = 0.5f;

        private float _fadeTimer = 0f;
        private bool _isFadingOut = false;
        private float _startVolume;

        void Start()
        {
            _startVolume = _audioSource.volume;
        }

        void Update()
        {
            if (_isFadingOut)
            {
                _fadeTimer += Time.deltaTime;
                _audioSource.volume = Mathf.Lerp(_startVolume, 0f, _fadeTimer / _FADE_DURATION);

                if (_fadeTimer >= _FADE_DURATION)
                {
                    _audioSource.Stop();
                    _audioSource.volume = _startVolume;
                    _isFadingOut = false;
                }
            }
        }
        
        public void SetAudioSource(AudioSource source)
        {
            _audioSource = source;
            _startVolume = _audioSource.volume;
        }

        public void StartFadeOut()
        {
            if (!_isFadingOut)
            {
                _fadeTimer = 0f;
                _startVolume = _audioSource.volume;
                _isFadingOut = true;
            }
        }
    }
}