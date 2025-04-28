using UnityEngine;

namespace Code.Scrips.ButtonMashing
{
    public class SingleButtonMashing : MashingTypeBase
    {
        public KeyCode buttonToPress = KeyCode.Space;  // Button to press for mashing

        private int _buttonPressCount = 0;

        public override void HandleMashing()
        {
            //this is only update based because i cant think of a good way rn
            if (Input.GetKeyDown(buttonToPress))  // If the correct button is pressed
            {
                _buttonPressCount++;
                Debug.Log("Button pressed: " + _buttonPressCount);
            }
        }
    }
}