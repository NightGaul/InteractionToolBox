using Code.ScriptableObjectScripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scrips
{
    public class FetchAndMatchManager : MonoBehaviour
    {
        public InteractEventSO interactEvent;
        public Camera cam;
        public Transform handTransform;
        private Transform _objectBeingHeld;
        public Transform objectGroup;

        private bool _holdingSomething;


        private void OnEnable()
        {
            interactEvent.onInteract += HandleInteract;
        }

        private void OnDisable()
        {
            interactEvent.onInteract -= HandleInteract;
        }

        private void HandleInteract(RaycastHit objHit, bool isInteractable)
        {
            var obj = objHit.transform;
            if (_holdingSomething)
            {
                Drop();
            }
            else if (isInteractable)
            {
                Grab(obj.transform);
            }
        }

        private void Grab(Transform objectToGrab)
        {
            Debug.Log("carry");
            objectToGrab.SetParent(handTransform, false);
            objectToGrab.transform.localPosition = Vector3.zero;
            _objectBeingHeld = objectToGrab;
            _holdingSomething = true;
        }

        private void Drop()
        {
            Debug.Log("huh");
            _objectBeingHeld.SetParent(objectGroup, true);
            _objectBeingHeld = null;
            _holdingSomething = false;
        }
    }
}