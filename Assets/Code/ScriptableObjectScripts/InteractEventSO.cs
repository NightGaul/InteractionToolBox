using UnityEngine;
using UnityEngine.Events;

namespace Code.ScriptableObjectScripts
{
    [CreateAssetMenu(fileName = "InteractEventSO", menuName = "Events/InteractEvent")]
    public class InteractEventSO : ScriptableObject
    {
        public UnityAction<RaycastHit, bool> onInteract;
        
        public void Raise(RaycastHit obj, bool isInteractable)
        {
            
            onInteract?.Invoke(obj, isInteractable);
        }
    }
}
