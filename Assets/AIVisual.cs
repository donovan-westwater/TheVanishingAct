using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class AIVisual : MonoBehaviour
{
    private BasicAi self;
    void OnDrawGizmosSelected()
    {
        self = this.gameObject.GetComponent<BasicAi>();
        Vector2 facing = this.gameObject.transform.GetChild(0).transform.position - this.transform.position;
      //  Debug.Log("I HAVE BEEN SELECTED!"+self.speed);
        // Display the explosion radius when selected
        //float angle = Mathf.Atan2(facing.x, facing.y) * Mathf.Rad2Deg;
        //        Vector3 start = new Vector3(10f * Mathf.Cos((angle + 45f / 2 )* Mathf.Deg2Rad), 10f * Mathf.Sin((angle + 45f / 2) * Mathf.Deg2Rad), 0);
        Vector3 start = Vector3.RotateTowards(facing, new Vector3(0, 1, 0),45f/2f*Mathf.Deg2Rad,10);
        Gizmos.DrawLine(self.transform.position, self.transform.position + start.normalized*10f);
        Gizmos.DrawLine(self.transform.position, self.transform.position + Vector3.RotateTowards(facing, new Vector3(0, 1, 0), -45f / 2f * Mathf.Deg2Rad, 10).normalized*10f);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireArc(self.transform.position, new Vector3(0,0,1), start.normalized, 45f, 10f);
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(self.transform.position, self.transform.forward, self.nextWaypointDistance);
        //try to get the script for the speafic ai. if it is found, then get thier speafic infomation
        //patrol: Draw lines between all the points Gizmos.drawAAline?
        //Coward: Alert radius should be drawn
        //Guard: Wall check radius should be drawn
    }
}
