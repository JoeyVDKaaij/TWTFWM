using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NonEditableAttribute))]
public class NonEditableDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false; // Disable editing
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;  // Re-enable editing after drawing
    }
}