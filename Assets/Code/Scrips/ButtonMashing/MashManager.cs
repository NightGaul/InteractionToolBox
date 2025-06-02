using Code.Scrips.Abstractions;
using Code.Scrips.ButtonMashing.MashingEffects;
using Code.Scrips.ButtonMashing.MashingTypes;
using Editors;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scrips.ButtonMashing
{
    public class MashManager : ManagerBase
    {
        [Header("Mashing Settings")] public MashingType mashingType;
        [ShowIfEnum("mashingType", MashingType.TIMED)]
        [Tooltip("Window of time for mashing")]
        public float mashingDuration;
        [Tooltip("Amount of keystrokes needed to trigger success")]
        public int requiredMashingAmount;
        [Header("Visuals")] public VisualEffect mashVisualEffect;
        [ShowIfEnum("mashVisualEffect", VisualEffect.SCREEN_SHAKE, VisualEffect.ZOOM)]
        [Range(0f, 1f)] 
        public float startIntensity;

        [ShowIfEnum("mashVisualEffect", VisualEffect.SCREEN_SHAKE, VisualEffect.ZOOM)]
        [Range(0f, 1f)] 
        public float endIntensity;
        
        
        private MashingEffectBase _currentEffectType;
        private MashingTypeBase _currentMashingType;
        [Header("Sounds")] public AudioClip mashSound;
        public AudioClip finishSound;
        private AudioSource _audioSourceFinish;
        
        private bool _alreadyWon;
        
        private void Start()
        {
            MashingTypeSetup();
            AudioSetup();
            VisualEffectsSetup();
        }
        private void Update()
        {
            if (_alreadyWon) return; 
            MashingExecution();
            
        }

        // Handles the core mashing logic; checks if success criteria are met and updates visual effects based on progress.
        private void MashingExecution()
        {
            var mashingAlreadyDone = _currentMashingType.HandleMashing();
            if (mashingAlreadyDone >= requiredMashingAmount)
            {
                Success();
            }

            if (!_currentEffectType) return;
            float strength = ((float)mashingAlreadyDone / requiredMashingAmount);

            _currentEffectType.ApplyMashingEffect(strength);
        }

        // Configures the appropriate visual effect (e.g., screen shake or zoom) and sets its intensity range.
        private void VisualEffectsSetup()
        {
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

            _currentEffectType.SetMashingIntensity(startIntensity, endIntensity);
        }

        // Sets up audio components, assigning sounds to the mashing type and final success clip.
        private void AudioSetup()
        {
            _audioSourceFinish = this.AddComponent<AudioSource>();
            _audioSourceFinish.clip = finishSound;
            _currentMashingType.SetMashingSound(mashSound);
        }

        // Instantiates the correct mashing behavior (e.g., timed, single, or alternate) and configures it as needed.
        private void MashingTypeSetup()
        {
            switch (mashingType)
            {
                case MashingType.SINGLE_BUTTON:
                    _currentMashingType = this.AddComponent<SingleButtonMashing>();
                    break;
                case MashingType.ALTERNATE_BUTTON:
                    _currentMashingType = this.AddComponent<AlternateButtonMashing>();
                    break;
                case MashingType.TIMED:
                    _currentMashingType = this.AddComponent<TimedButtonMashing>();
                    if (_currentMashingType is TimedButtonMashing timedMashing)
                    {
                        timedMashing.SetMashingDuration(mashingDuration); // 5 seconds duration
                    }
                    break;
                default:
                    _currentMashingType = this.AddComponent<SingleButtonMashing>();
                    break;
            }
        }

        // Cleans up and destroys all runtime components to avoid memory leaks or dangling references.
        private void OnDestroy()
        {
            if (_currentEffectType) Destroy(_currentEffectType.gameObject);
            if(_currentMashingType)Destroy(_currentMashingType.gameObject);
            Destroy(_audioSourceFinish);
        }

        // Called upon successful mashing; plays the finish sound and prevents further input.
        public override void Success()
        {
            _audioSourceFinish.Play();
            _alreadyWon = true;
        }
    }
}