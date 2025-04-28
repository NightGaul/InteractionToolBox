using System;
using Code.ScriptableObjects;
using Code.ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scrips
{
    public class InputListener : MonoBehaviour
    {
        public RightClickEventSO clickEvent;
        public DragEventSO dragEvent;
        public InteractEventSO interactEvent;
        public ScrollEventSO scrollEvent;

        private bool _isDragging;
        private GameObject _draggedObject;

        //TO DO:
        //Dynamic Input, save with scriptable Object


        void Update()
        {
            // Left click
            if (Input.GetMouseButtonDown(0))
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

            // Right CLick
            if (Input.GetMouseButton(1))
            {
                Vector3 mousePosition = Input.mousePosition;
                clickEvent.Raise(mousePosition);
            }

            if (_isDragging)
            {
                Vector3 mousePosition = Input.mousePosition;
                dragEvent.UpdateDrag(_draggedObject, mousePosition);
            }

            // Release click
            if (Input.GetMouseButtonUp(0))
            {
                if (_isDragging)
                {
                    dragEvent.EndDrag(_draggedObject);
                    _isDragging = false;
                    _draggedObject = null;
                }
            }

            // Interact Key
            if (Input.GetKeyUp(KeyCode.E))
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Interactable"))
                    {
                        interactEvent.Raise(hit, true);
                    }
                    else
                    {
                        interactEvent.Raise(hit, false);
                    }
                }
            }

            //Scroll Wheel
            //mouse Scroll Delta either 1,0,-1
            if (Input.mouseScrollDelta.y != 0)
            {
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Scrollable"))
                    {
                        scrollEvent.Raise(Input.mouseScrollDelta.y);
                    }
                }
            }
            
        }
    }
}