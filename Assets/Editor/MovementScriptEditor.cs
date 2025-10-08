using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovementScript))]
public class MovementScriptEditor : Editor
{
    private SerializedProperty movementSpeedProp;
    private SerializedProperty movementTypeProp;
    private SerializedProperty axisProp;
    private SerializedProperty reverseMovementProp;
    private SerializedProperty moveInWorldSpaceProp;
    private SerializedProperty targetProp;
    private SerializedProperty pathPointsProp;
    private SerializedProperty loopPathProp;
    private SerializedProperty destroyOnLastPointProp;

    private void OnEnable()
    {
        movementSpeedProp = serializedObject.FindProperty("movementSpeed");
        movementTypeProp = serializedObject.FindProperty("movementType");
        axisProp = serializedObject.FindProperty("axis");
        reverseMovementProp = serializedObject.FindProperty("reverseMovement");
        moveInWorldSpaceProp = serializedObject.FindProperty("moveInWorldSpace");
        targetProp = serializedObject.FindProperty("target");
        pathPointsProp = serializedObject.FindProperty("pathPoints");
        loopPathProp = serializedObject.FindProperty("loopPath");
        destroyOnLastPointProp = serializedObject.FindProperty("destroyOnLastPoint");
    }

    public override void OnInspectorGUI()
    {
        MovementType movementType = (MovementType)movementTypeProp.enumValueIndex;
        serializedObject.Update();

        // Draw default inspector property fields
        EditorGUILayout.PropertyField(movementSpeedProp);
        EditorGUILayout.PropertyField(movementTypeProp);

        if (movementType == MovementType.oneAxisMovement)
        {
            EditorGUILayout.PropertyField(axisProp);
            EditorGUILayout.PropertyField(reverseMovementProp);
            EditorGUILayout.PropertyField(moveInWorldSpaceProp);
        }
        else if (movementType == MovementType.pathFinding)
        {
            EditorGUILayout.PropertyField(targetProp);
        }
        else if (movementType == MovementType.staticPath)
        {
            EditorGUILayout.PropertyField(pathPointsProp);
            EditorGUILayout.PropertyField(loopPathProp);

            if (loopPathProp.boolValue == true)
            {
                GUI.enabled = false;
                destroyOnLastPointProp.boolValue = false;
            }
            EditorGUILayout.PropertyField(destroyOnLastPointProp);
            if (loopPathProp.boolValue == true) GUI.enabled = true;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
