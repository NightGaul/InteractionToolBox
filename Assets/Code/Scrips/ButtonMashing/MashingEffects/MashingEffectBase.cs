using UnityEngine;

namespace Code.Scrips.ButtonMashing.MashingEffects
{
    public abstract class MashingEffectBase: MonoBehaviour
    {
        public abstract void ApplyMashingEffect(float f);
        
        public abstract void SetMashingIntensity(float startIntensity,float endIntensity);
        public abstract float Remap(float value, float inMin, float inMax, float outMin, float outMax);
    }
}