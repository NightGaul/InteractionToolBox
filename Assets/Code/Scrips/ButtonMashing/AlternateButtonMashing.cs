using UnityEngine;

namespace Code.Scrips.ButtonMashing
{
    public class AlternateButtonMashing : MashingTypeBase
    {
        public KeyCode primaryButton = KeyCode.A;
        public KeyCode secondaryButton = KeyCode.D;

        private int _buttonPressCount = 0;
        private bool _lastPressed = true;

        public override void HandleMashing()
        {
            if (_lastPressed && Input.GetKeyDown(primaryButton)) // If primary button is pressed
            {
                _buttonPressCount++;
                Debug.Log("Primary button pressed: " + _buttonPressCount);
                _lastPressed = false;
            }
            else if (!_lastPressed && Input.GetKeyDown(secondaryButton)) // If secondary button is pressed
            {
                _buttonPressCount++;
                Debug.Log("Secondary button pressed: " + _buttonPressCount);
                _lastPressed = true;
            }
        }
    }
}