using System;
using Code.ScriptableObjectScripts;
using Unity.VisualScripting;
using UnityEngine;
using Code.ScriptableObjects;

namespace Code.Scrips.ButtonMashing.MashingTypes
{
    public class SingleButtonMashing : MashingTypeBase
    {
        private InputSettingsSo _inputSettingsSo;
        private KeyCode _buttonToPress;  
        
        private int _requiredMashingAmount;
        private int _buttonPressCount = 0;

        private AudioClip _mashSound;
        private AudioSource _audioSourceMash;

        private void Start()
        {
            _inputSettingsSo = Resources.Load<InputSettingsSo>("SettingSO/InputSettings");
            _buttonToPress = _inputSettingsSo.singleMashInput;
        }

        public override int HandleMashing()
        {
            //this is only update based because i cant think of a good way rn
            if (Input.GetKeyDown(_buttonToPress)) 
            {
                _buttonPressCount++;
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