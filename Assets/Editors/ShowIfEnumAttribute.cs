using UnityEngine;

namespace Editors
{
    public class ShowIfEnumAttribute : PropertyAttribute
    {
        public string EnumFieldName { get; private set; }
        public object[] EnumValues { get; private set; }

        public ShowIfEnumAttribute(string enumFieldName, params object[] enumValues)
        {
            EnumFieldName = enumFieldName;
            EnumValues = enumValues;
        }
    }
}