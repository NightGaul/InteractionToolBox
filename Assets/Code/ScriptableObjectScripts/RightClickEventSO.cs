using UnityEngine;
using UnityEngine.Events;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/Input Event")]
    public class RightClickEventSO : ScriptableObject
    {
        public UnityAction<Vector3> onInputReceived;

        public void Raise(Vector3 position)
        {
            onInputReceived?.Invoke(position);
        }
    }
}