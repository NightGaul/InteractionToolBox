using Code.ScriptableObjectScripts;
using UnityEngine;

namespace Code.Scrips
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public Transform target;
        public ThirdPersonCameraSettingsSO settings;

        private float _yaw = 0f;
        private float _pitch = 0f;

        void LateUpdate()
        {
            if (target == null || settings == null) return;

            _yaw += Input.GetAxis("Mouse X") * settings.mouseSensitivity;
            _pitch -= Input.GetAxis("Mouse Y") * settings.mouseSensitivity;
            _pitch = Mathf.Clamp(_pitch, settings.minY, settings.maxY);

            Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            Vector3 direction = rotation * Vector3.back;

            transform.position = target.position + Vector3.up * settings.heightOffset + direction * settings.distance;
            transform.LookAt(target.position + Vector3.up * settings.heightOffset);
        }
    }
}