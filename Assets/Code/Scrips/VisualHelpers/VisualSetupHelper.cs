using UnityEngine;

namespace Code.Scrips.VisualHelpers
{
    public static class VisualSetupHelper
    {
        public static void AddOutlineComponents(Transform obj, OutlineMode mode, Color color, float width)
        {
            var outline = obj.gameObject.AddComponent<Outline>();
            outline.OutlineColor = color;
            outline.OutlineWidth = width;

            switch (mode)
            {
                case OutlineMode.ALWAYS:
                    obj.gameObject.AddComponent<OutlineAlways>();
                    break;
                case OutlineMode.ON_HOVER:
                    obj.gameObject.AddComponent<OutlineOnHover>();
                    break;
                case OutlineMode.NONE:
                    break;
            }
        }
    }
}