using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scrips.ButtonMashing.MashingTypes
{
    public class AlternateButtonMashing : MashingTypeBase
    {
        public KeyCode primaryButton = KeyCode.A;
        public KeyCode secondaryButton = KeyCode.D;
        
        private int _requiredMashingAmount;
        
        private int _buttonPressCount = 0;
        private bool _lastPressed = true;

        private AudioClip _mashSound;
        private AudioSource _audioSourceMash;

       
        public override int HandleMashing()
        {
            if (_lastPressed && Input.GetKeyDown(primaryButton)) // If primary button is pressed
            {
                _buttonPressCount++;
                Debug.Log("Primary button pressed: " + _buttonPressCount);
                _lastPressed = false;
                _audioSourceMash.PlayOneShot(_mashSound);
            }
            else if (!_lastPressed && Input.GetKeyDown(secondaryButton)) // If secondary button is pressed
            {
                _buttonPressCount++;
                Debug.Log("Secondary button pressed: " + _buttonPressCount);
                _lastPressed = true;
                _audioSourceMash.PlayOneShot(_mashSound);
            }
            
            return _buttonPressCount;
        }

        public override void SetMashingSound(AudioClip mashingSound)
        {
            _audioSourceMash = this.AddComponent<AudioSource>();
            _mashSound = mashingSound;
        }

        private void OnDestroy()
        {
            Destroy(_audioSourceMash);
        }
    }
}