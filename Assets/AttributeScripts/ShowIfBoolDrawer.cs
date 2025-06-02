using UnityEditor;
using UnityEngine;

namespace Editors
{
    [CustomPropertyDrawer(typeof(ShowIfBoolAttribute))]
    public class ShowIfBoolDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldShow(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ShouldShow(property)
                ? EditorGUI.GetPropertyHeight(property, label, true)
                : 0f;
        }

        private bool ShouldShow(SerializedProperty property)
        {
            var condition = (ShowIfBoolAttribute)attribute;
            var target = property.serializedObject;

            var boolProp = target.FindProperty(condition.boolFieldName);
            return boolProp != null && boolProp.propertyType == SerializedPropertyType.Boolean && boolProp.boolValue;
        }
    }
}