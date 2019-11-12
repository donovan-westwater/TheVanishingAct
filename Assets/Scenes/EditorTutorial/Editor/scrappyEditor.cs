using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(editModeTest))]
public class scrappyEditor : Editor
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public override void OnInspectorGUI()
    {
        editModeTest obj = (editModeTest)serializedObject.targetObject;
        serializedObject.Update();
        obj.speed = EditorGUILayout.FloatField("Speed", obj.speed);
        serializedObject.ApplyModifiedProperties();
    }
}
