using UnityEngine;

namespace Code.Scrips.ButtonMashing.MashingTypes
{
    public abstract class MashingTypeBase : MonoBehaviour
    {
        public abstract int HandleMashing();  // Abstract method to handle mashing logic
        
        public abstract void SetMashingSound(AudioClip mashingSound);
    }
}