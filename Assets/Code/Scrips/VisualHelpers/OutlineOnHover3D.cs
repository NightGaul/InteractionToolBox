using System;
using Code.Scrips.FetchAndMatch;
using UnityEngine;

namespace Code.Scrips.VisualHelpers
{
    public class OutlineOnHover3D : MonoBehaviour
    {
        private Outline _outline;
        private Camera _camera;

        void Start()
        {
            _camera = Camera.main;
            
            _outline = GetComponent<Outline>();
            if (_outline != null)
            {
                _outline.enabled = false; // Ensure it's off at start
            }
            else
            {
                Debug.LogWarning("Outline component not found on " + gameObject.name);
            }
        }

        private void Update()
        {
            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.GetComponent<DropOffPoint>() != null)
                {
                    _outline.enabled = true;
                }
                else
                {
                    _outline.enabled = false;
                }
            }
        }
    }
}