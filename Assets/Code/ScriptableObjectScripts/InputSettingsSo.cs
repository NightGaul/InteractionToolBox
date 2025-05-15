using UnityEngine;

namespace Code.ScriptableObjectScripts
{
    [CreateAssetMenu(fileName = "NewInputSettings", menuName = "Settings/InputSettingsSO")]
    public class InputSettingsSo: ScriptableObject
    {
        public KeyCode selectInput;
        public KeyCode paintInput;
        public KeyCode interactInput;
        public KeyCode singleMashInput;
        public KeyCode multiMashInputA;
        public KeyCode multiMashInputB;
    }
}