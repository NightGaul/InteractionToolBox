using UnityEngine;

namespace Code.Scrips.ButtonMashing
{
    public class MashManager : MonoBehaviour
    {
        public MashingTypeBase currentMashingType;  // Reference to the current mashing type class


        void Update()
        {
            currentMashingType.HandleMashing();
        }
    }
}