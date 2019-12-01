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
        Gizmos.color = Color.red;
        Gizmos.DrawLine(self.transform.position, self.transform.position + start.normalized*10f);
        Gizmos.DrawLine(self.transform.position, self.transform.position + Vector3.RotateTowards(facing, new Vector3(0, 1, 0), -45f / 2f * Mathf.Deg2Rad, 10).normalized*10f);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireArc(self.transform.position, new Vector3(0,0,1), start.normalized, 45f, 10f);
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(self.transform.position, self.transform.forward, self.nextWaypointDistance);
        //try to get the script for the speafic ai. if it is found, then get thier speafic infomation
        //patrol: Draw lines between all the points Gizmos.drawAAline?
        PatrolAi check = this.gameObject.GetComponent<PatrolAi>();
        if (check != null)
        {
            Gizmos.color = new Color(0.5f,0.05f,0.75f,1f);
            Vector3 origin = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            foreach(Transform t in check.targets)
            {
                if (t == null) continue;
                Vector3 end = new Vector3(t.position.x, t.position.y, origin.z);
                Gizmos.DrawLine(origin, t.position);
                origin = t.position;
            }
            return;
        }
        //Guard: Wall check radius should be drawn
        GuardAi guard = this.gameObject.GetComponent<GuardAi>();
        if(guard != null)
        {
            Gizmos.color = new Color(0.5f, 0.05f, 0.75f, 1f);
            Gizmos.DrawLine(this.transform.position, this.transform.position + guard.wallcheck*new Vector3(facing.x,facing.y,0).normalized);
            Vector3 checkLeft = Vector3.RotateTowards(facing, new Vector3(0, 1, 0), 90f * Mathf.Deg2Rad,guard.wallcheck);
            Gizmos.DrawLine(this.transform.position, this.transform.position + checkLeft.normalized* guard.wallcheck);
            Vector3 checkBehind = Vector3.RotateTowards(facing, -facing, 180f * Mathf.Deg2Rad, guard.wallcheck);
            Gizmos.DrawLine(this.transform.position, this.transform.position + checkBehind.normalized * guard.wallcheck);
            Vector3 checkRight = Vector3.RotateTowards(facing, new Vector3(0, 1, 0), -90f * Mathf.Deg2Rad, guard.wallcheck);
            Gizmos.DrawLine(this.transform.position, this.transform.position + checkRight.normalized * guard.wallcheck);
            return;
        }
        //Coward: Alert radius should be drawn 
        CowardAi coward = this.gameObject.GetComponent<CowardAi>();
        if(coward != null)
        {
            Handles.color = new Color(0.5f, 0.05f, 0.75f, 1f);
            UnityEditor.Handles.DrawWireDisc(self.transform.position, self.transform.forward, 20f ); //coward.alertRadis
            Handles.color = new Color(0.05f, 0.05f, 0.75f, 1f);
            UnityEditor.Handles.DrawWireDisc(self.transform.position, self.transform.forward,60f);   //coward.searchRadis

        }
        
        
    }
    
}
