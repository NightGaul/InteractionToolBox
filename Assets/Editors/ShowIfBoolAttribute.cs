using UnityEngine;

namespace Editors
{
    public class ShowIfBoolAttribute : PropertyAttribute
    {
        public string boolFieldName;

        public ShowIfBoolAttribute(string boolFieldName)
        {
            this.boolFieldName = boolFieldName;
        }
    }
}