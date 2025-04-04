using UnityEngine;
using UnityEngine.Events;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/Drag Event")]
    public class DragEventSO : ScriptableObject
    {
        public UnityAction<GameObject> onDragStart;
        public UnityAction<GameObject, Vector3> onDragUpdate;
        public UnityAction<GameObject> onDragEnd;

        public void StartDrag(GameObject obj) => onDragStart?.Invoke(obj);
        public void UpdateDrag(GameObject obj, Vector3 position) => onDragUpdate?.Invoke(obj, position);
        public void EndDrag(GameObject obj) => onDragEnd?.Invoke(obj);
    }
}