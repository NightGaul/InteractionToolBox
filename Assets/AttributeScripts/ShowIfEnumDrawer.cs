using UnityEditor;
using UnityEngine;

namespace Editors
{
    [CustomPropertyDrawer(typeof(ShowIfEnumAttribute))]
    public class ShowIfEnumDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfEnumAttribute condition = (ShowIfEnumAttribute)attribute;

            if (ShouldShow(property, condition))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfEnumAttribute condition = (ShowIfEnumAttribute)attribute;
            return ShouldShow(property, condition)
                ? EditorGUI.GetPropertyHeight(property, label, true)
                : 0f;
        }

        private bool ShouldShow(SerializedProperty property, ShowIfEnumAttribute condition)
        {
            SerializedObject target = property.serializedObject;
            SerializedProperty enumProp = target.FindProperty(condition.EnumFieldName);

            if (enumProp == null || enumProp.propertyType != SerializedPropertyType.Enum)
                return false;

            string currentEnumName = enumProp.enumNames[enumProp.enumValueIndex];

            foreach (var value in condition.EnumValues)
            {
                if (value.ToString() == currentEnumName)
                    return true;
            }

            return false;
        }
    }
}