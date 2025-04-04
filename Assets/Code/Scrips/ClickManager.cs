using Code.ScriptableObjects;
using UnityEngine;

namespace Code.Scrips
{
    public class ClickManager : MonoBehaviour
    {
        public InputEventSO clickEvent;

        void OnEnable()
        {
            clickEvent.onInputReceived += HandleClick;
        }

        void OnDisable()
        {
            clickEvent.onInputReceived -= HandleClick;
        }

        private void HandleClick(Vector3 position)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Draggable"))
                {
                    Debug.Log("Clicked on: " + hit.collider.name);
                   
                }
            }
        }
    }
}