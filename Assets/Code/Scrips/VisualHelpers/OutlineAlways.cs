using UnityEngine;

namespace Code.Scrips.VisualHelpers
{
    public class OutlineAlways : MonoBehaviour
    {
        private Outline _outline;

        void Start()
        {
            _outline = GetComponent<Outline>();
            _outline.enabled = true;
        }
    }
}
