using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scrips.ButtonMashing.MashingTypes
{
    public class SingleButtonMashing : MashingTypeBase
    {
        public KeyCode buttonToPress = KeyCode.Space;  // Button to press for mashing
        
        private int _requiredMashingAmount;
        private int _buttonPressCount = 0;

        private AudioClip _mashSound;
        private AudioSource _audioSourceMash;
        
        
        public override int HandleMashing()
        {
            //this is only update based because i cant think of a good way rn
            if (Input.GetKeyDown(buttonToPress)) 
            {
                _buttonPressCount++;
                //Debug.Log("Button pressed: " + _buttonPressCount);
                _audioSourceMash.PlayOneShot(_mashSound);
            }
            
            return _buttonPressCount;
        }

        public override void SetMashingSound(AudioClip mashingSound)
        {
            _audioSourceMash = this.AddComponent<AudioSource>();
            _mashSound = mashingSound;
        }

        
    }
}