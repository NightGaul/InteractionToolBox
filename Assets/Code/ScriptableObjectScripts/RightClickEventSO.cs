using UnityEngine;
using UnityEngine.Events;

namespace Code.ScriptableObjectScripts
{
    [CreateAssetMenu(menuName = "Events/Input Event")]
    public class RightClickEventSO : ScriptableObject
    {
        public UnityAction<Vector3> onInputReceived;
        public UnityAction onInputStop;

        public void Raise(Vector3 position)
        {
            onInputReceived?.Invoke(position);
        }

        public void Raise()
        {
            onInputStop?.Invoke();
        }
    }
}