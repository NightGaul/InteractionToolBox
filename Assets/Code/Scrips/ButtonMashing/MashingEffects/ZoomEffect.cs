using UnityEngine;

namespace Code.Scrips.ButtonMashing.MashingEffects
{
    public class ZoomEffect : MashingEffectBase
    {
        private float _start;
        private float _end = 30f;
        private Camera _camera;
        
        private readonly float _minCap = 10;
        private readonly float _maxCap = 100;
        
        void Start()
        {
            _camera = Camera.main;
        }

        public override void ApplyMashingEffect(float effectStrength)
        {
            if (effectStrength >= 1f)
            {
                Zoom(1);
                return;
            }

            Zoom(effectStrength);
        }

        public override void SetMashingIntensity(float startIntensity, float endIntensity)
        {
            _start = Remap(startIntensity,0,1, _minCap, _maxCap);
            _end = Remap(endIntensity,0,1, _minCap, _maxCap);;
        }

        private void Zoom(float effectStrength)
        {
            _camera.fieldOfView = Mathf.Lerp(_end, _start, effectStrength);
        }
        
        public override float Remap(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
        }
    }
}