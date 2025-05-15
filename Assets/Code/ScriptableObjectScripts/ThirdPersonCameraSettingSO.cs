using UnityEngine;

namespace Code.ScriptableObjectScripts
{
    [CreateAssetMenu(fileName = "NewCameraSettings", menuName = "Settings/ThirdPersonSettingsSO")]
    public class ThirdPersonCameraSettingsSO : ScriptableObject
    {
        public float mouseSensitivity = 3f;
        public float distance = 5f;
        public float heightOffset = 1.5f;
        public float minY = -20f;
        public float maxY = 40f;
    }
}