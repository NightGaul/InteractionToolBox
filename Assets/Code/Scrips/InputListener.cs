using Code.ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scrips
{
    public class InputListener : MonoBehaviour
    {
        [Header("Events")]
        public RightClickEventSO clickEvent;
        public DragEventSO dragEvent;
        public InteractEventSO interactEvent;
        public ScrollEventSO scrollEvent;
        
        [FormerlySerializedAs("settings")] [Header("InputSettings")]
        public InputSettingsSo settingsSo;
        private bool _isDragging;
        private GameObject _draggedObject;
        private Camera _camera;
        
        private void Start()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            
            if (Input.GetKey(settingsSo.paintInput))
            {
                Vector3 mousePosition = Input.mousePosition;
                clickEvent.Raise(mousePosition);
            }
            if (Input.GetKeyUp(settingsSo.paintInput))
            {
                clickEvent.Raise();
            }
            
            if (Input.GetKeyDown(settingsSo.selectInput))
            {
                Vector3 mousePosition = Input.mousePosition;


                Ray ray = _camera.ScreenPointToRay(mousePosition);
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
            

            if (_isDragging)
            {
                Vector3 mousePosition = Input.mousePosition;
                dragEvent.UpdateDrag(_draggedObject, mousePosition);
            }

            // Release click
            if (Input.GetKeyUp(settingsSo.selectInput))
            {
                if (_isDragging)
                {
                    dragEvent.EndDrag(_draggedObject);
                    _isDragging = false;
                    _draggedObject = null;
                }
            }

            // Interact Key
            if (Input.GetKeyUp(settingsSo.interactInput))
            {
                if (_camera != null)
                {
                    Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
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
            }
            
            //Scroll Wheel -> not to be changed
            //mouse Scroll Delta either 1,0,-1
            if (Input.mouseScrollDelta.y != 0)
            {
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = _camera.ScreenPointToRay(mousePosition);
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