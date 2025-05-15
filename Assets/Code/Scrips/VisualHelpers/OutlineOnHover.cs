using UnityEngine;

namespace Code.Scrips.VisualHelpers
{
    public class OutlineOnHover : MonoBehaviour
    {
        private Outline _outline;

        void Start()
        {
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

        void OnMouseEnter()
        {
            if (_outline != null)
            {
                _outline.enabled = true;
            }
        }

        void OnMouseExit()
        {
            if (_outline != null)
            {
                _outline.enabled = false;
            }
        }
    }
}