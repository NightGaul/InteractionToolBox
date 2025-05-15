using System;
using Code.Scrips.ButtonMashing.MashingEffects;
using Code.Scrips.ButtonMashing.MashingTypes;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scrips.ButtonMashing
{
    public class MashManager : MonoBehaviour
    {
        [Header("Mashing Settings")] public MashingType mashingType;
        public int requiredMashingAmount;
        [Header("Visuals")] public VisualEffect mashVisualEffect;
        [Header("Sounds")] public AudioClip mashSound;
        public AudioClip finishSound;
        private AudioSource _audioSourceFinish;

        private bool _alreadyWon;
        //TODO: Get a slider or smth, its a bit annoying to get this to the right value

        [Range(0f,1f)]
        public float mashEffectIntensityStart;
        [Range(0f,1f)]
        public float mashEffectIntensityEnd;
        private MashingEffectBase _currentEffectType;

        private MashingTypeBase _currentMashingType; // Reference to the current mashing type class

        private void Start()
        {
            switch (mashingType)
            {
                //this might be problematic, because you cant enter the right keys
                case MashingType.SINGLE_BUTTON:
                    _currentMashingType = this.AddComponent<SingleButtonMashing>();
                    break;
                case MashingType.ALTERNATE_BUTTON:
                    _currentMashingType = this.AddComponent<AlternateButtonMashing>();
                    break;
            }

            _currentMashingType.SetMashingSound(mashSound);
            _audioSourceFinish   = this.AddComponent<AudioSource>();
            _audioSourceFinish.clip = finishSound;
            
            switch (mashVisualEffect)
            {
                case VisualEffect.SCREEN_SHAKE:
                    _currentEffectType = this.AddComponent<ScreenShakeEffect>();
                    break;
                case VisualEffect.ZOOM:
                    _currentEffectType = this.AddComponent<ZoomEffect>();
                    break;
                case VisualEffect.NONE:
                    _currentEffectType = null;
                    return;
            }

            _currentEffectType.SetMashingIntensity(mashEffectIntensityStart, mashEffectIntensityEnd);
        }

        void Update()
        {
            if (_alreadyWon) return; //makes it less taxing for system
            
            //this is an inefficient way, but I'll have to ask someone else because I'm kinda stuck with this solution
            var mashingAlreadyDone = _currentMashingType.HandleMashing();
            if (mashingAlreadyDone >= requiredMashingAmount)
            {
                _audioSourceFinish.Play();
                Debug.Log("u win");
                _alreadyWon = true;
                //Add Win effect
            }

            if (!_currentEffectType) return;
            float strength = ((float)mashingAlreadyDone / requiredMashingAmount);

            _currentEffectType.ApplyMashingEffect(strength);
        }

        private void OnDestroy()
        {
            Destroy(_currentEffectType.gameObject);
            Destroy(_currentMashingType.gameObject);
        }
    }
}