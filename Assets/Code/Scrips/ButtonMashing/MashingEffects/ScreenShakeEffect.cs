using UnityEngine;

namespace Code.Scrips.ButtonMashing.MashingEffects
{
    public class ScreenShakeEffect : MashingEffectBase
    {
        private float _start; // Minimum shake intensity
        private float _end; // Maximum shake intensity

        private readonly float _minCap = 0;
        private readonly float _maxCap = 3;
        
        private Camera _camera;
        private Vector3 _originalPosition;

        void Start()
        {
            _camera = Camera.main;
            _originalPosition = _camera.transform.localPosition;
        }

        public override void ApplyMashingEffect(float effectStrength)
        {
            if (effectStrength >= 1f)
            {
                ApplyShake(1f);
                return;
            }

            ApplyShake(effectStrength);
        }

        public override void SetMashingIntensity(float startIntensity, float endIntensity)
        {   
            
            _start = Remap(startIntensity,0,1, _minCap, _maxCap);
            _end = Remap(endIntensity,0,1, _minCap, _maxCap);
        }

        private void ApplyShake(float effectStrength)
        {
            float shakeAmount = Mathf.Lerp(_start, _end, effectStrength);
            Vector3 randomOffset = Random.insideUnitSphere * shakeAmount;

            _camera.transform.localPosition = _originalPosition + randomOffset;
        }

        public override float Remap(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
        }
        void OnDisable()
        {
            if (_camera != null)
                _camera.transform.localPosition = _originalPosition;
        }
        
    }
}