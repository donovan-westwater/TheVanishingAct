using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class AIEditor : MonoBehaviour
{
    //Should be able to create/ destory AIs in there entirity (Optional) 
    //Should be able to adjust patrol paths of Patrol ai (Optional)
    //Visualization will be handled in the AI classes under onDrawSelected
    //Should be able to select all Ai
    private bool editMode = true;
    private void Start()
    {
        editMode = false;
    }
    [MenuItem("GameObject/Add Node to Patrol Path", true,-100)]
    static bool isPatrol()
    {
        PatrolAi test = Selection.activeGameObject.GetComponent<PatrolAi>();
        if (test != null)
        {
            return true;
        }
        return false;
    }
    [MenuItem("GameObject/Add Node to Patrol Path", false,-100)]
    static void addNodeToPath()
    {
        PatrolAi test = Selection.activeGameObject.GetComponent<PatrolAi>();
        Transform newSelect = test.addToPath();
        Selection.activeGameObject = newSelect.gameObject;
    }
    

}
