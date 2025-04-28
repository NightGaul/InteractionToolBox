using UnityEngine;
using UnityEngine.Events;

namespace Code.ScriptableObjectScripts
{
    [CreateAssetMenu (menuName = "Events/Scroll Event")]
    public class ScrollEventSO: ScriptableObject
    {
        public UnityAction<float> onScroll;
        public void Raise(float delta)
        {
            onScroll?.Invoke(delta);
        }
    }
}