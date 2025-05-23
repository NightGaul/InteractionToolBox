using Code.ScriptableObjectScripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scrips.ButtonMashing.MashingTypes
{
    public class TimedButtonMashing : MashingTypeBase
    {
        private InputSettingsSo _inputSettingsSo;
        private KeyCode _buttonToPress;

        private float _mashDuration = 5f;
        private float _startTime;
        private int _pressCount;
        private bool _timerStarted;

        private AudioClip _mashSound;
        private AudioSource _audioSourceMash;

        private void Start()
        {
            _inputSettingsSo = Resources.Load<InputSettingsSo>("SettingSO/InputSettings");
            _buttonToPress = _inputSettingsSo.singleMashInput;
            _timerStarted = false;
        }

        public override int HandleMashing()
        {
            
            
            if (Time.time - _startTime <= _mashDuration)
            {
                if (Input.GetKeyDown(_buttonToPress))
                {
                    if (!_timerStarted)
                    {
                        _startTime = Time.time;
                        _timerStarted = true;
                    }
                    _pressCount++;
                    _audioSourceMash.PlayOneShot(_mashSound);
                }
            }
            else
            {
                //this lets you retry
                _startTime = Time.time;
                _timerStarted = true;
            }
            

            return _pressCount;
        }

        public void SetMashingDuration(float mashDuration)
        {
            _mashDuration = mashDuration;
        }
        public override void SetMashingSound(AudioClip mashingSound)
        {
            _audioSourceMash = this.AddComponent<AudioSource>();
            _mashSound = mashingSound;
        }
    }
}
