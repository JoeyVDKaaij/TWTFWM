using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShootScript))]
public class ShootScriptEditor : Editor
{
    private SerializedProperty fireModeProp;
    private SerializedProperty fireRateProp;
    private SerializedProperty rangeProp;
    private SerializedProperty bulletProp;
    private SerializedProperty aoeDamageProp;
    private SerializedProperty aoeGameObjectProp;
    private SerializedProperty slowSpeedProp;
    private SerializedProperty debufTimerProp;
    private SerializedProperty absoluteSlowSpeedProp;
    private SerializedProperty debufGameObjectProp;

    private void OnEnable()
    {
        fireModeProp = serializedObject.FindProperty("fireMode");
        fireRateProp = serializedObject.FindProperty("fireRate");
        rangeProp = serializedObject.FindProperty("range");
        bulletProp = serializedObject.FindProperty("bullet");
        aoeDamageProp = serializedObject.FindProperty("aoeDamage");
        aoeGameObjectProp = serializedObject.FindProperty("aoeGameObject");
        slowSpeedProp = serializedObject.FindProperty("slowSpeed");
        debufTimerProp = serializedObject.FindProperty("debufTimer");
        absoluteSlowSpeedProp = serializedObject.FindProperty("absoluteSlowSpeed");
        debufGameObjectProp = serializedObject.FindProperty("debufGameObject");
    }

    public override void OnInspectorGUI()
    {
        FireMode fireMode = (FireMode)fireModeProp.enumValueIndex;
        serializedObject.Update();

        // Draw default inspector property fields
        EditorGUILayout.PropertyField(fireModeProp);
        //if (fireMode != FireMode.debuf)
            EditorGUILayout.PropertyField(fireRateProp);
        EditorGUILayout.PropertyField(rangeProp);

        switch (fireMode)
        {
            case FireMode.single:
                EditorGUILayout.PropertyField(bulletProp);
                break;
            case FireMode.aoe:
                EditorGUILayout.PropertyField(aoeDamageProp);
                EditorGUILayout.PropertyField(aoeGameObjectProp);
                break;
            case FireMode.debuf:
                EditorGUILayout.PropertyField(slowSpeedProp);
                EditorGUILayout.PropertyField(debufTimerProp);
                EditorGUILayout.PropertyField(absoluteSlowSpeedProp);
                EditorGUILayout.PropertyField(debufGameObjectProp);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
