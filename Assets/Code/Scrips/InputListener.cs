using Code.ScriptableObjects;
using UnityEngine;

namespace Code.Scrips
{
    public class InputListener : MonoBehaviour
    {
        public InputEventSO clickEvent;
        public DragEventSO dragEvent;

        private bool _isDragging;
        private GameObject _draggedObject;

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Left click
            {
                Vector3 mousePosition = Input.mousePosition;
                

                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Draggable"))
                    {
                        _isDragging = true;
                        _draggedObject = hit.collider.gameObject;
                        dragEvent.StartDrag(_draggedObject);
                    }
                }
            }

            if (Input.GetMouseButtonDown(1)) // Right CLick
            {
                Vector3 mousePosition = Input.mousePosition;
                clickEvent.Raise(mousePosition);
            }

            if (_isDragging)
            {
                Vector3 mousePosition = Input.mousePosition;
                dragEvent.UpdateDrag(_draggedObject, mousePosition);
            }

            if (Input.GetMouseButtonUp(0)) // Release click
            {
                if (_isDragging)
                {
                    dragEvent.EndDrag(_draggedObject);
                    _isDragging = false;
                    _draggedObject = null;
                }
            }
        }
    }
}