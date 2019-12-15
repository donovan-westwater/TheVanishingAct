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
        //Vector3 start = Vector3.RotateTowards(facing, new Vector3(0, 1, 0),45f/2f*Mathf.Deg2Rad,10);
        // new code here
        Vector3 normal = facing;
        Vector3 outTan = new Vector3(0, 1, 0);
        Vector3.OrthoNormalize(ref normal, ref outTan);
        //new code end
        Vector3 start = Vector3.RotateTowards(facing, outTan,45f/2f*Mathf.Deg2Rad,10);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(self.transform.position, self.transform.position + start.normalized*10f);
        //Gizmos.color = Color.blue;
        Gizmos.DrawLine(self.transform.position, self.transform.position + Vector3.RotateTowards(facing,outTan, -45f / 2f * Mathf.Deg2Rad, 10).normalized*10f);
        UnityEditor.Handles.color = Color.red;
        //new code start
        Vector3 from = start;
        Vector3 fromA = Vector3.RotateTowards(facing, outTan, -45f / 2f * Mathf.Deg2Rad, 10);
        float dir = Vector3.Cross(fromA, start).z;
        if (Vector3.Cross(fromA, start).z > 0) from = Vector3.RotateTowards(facing, outTan, -45f / 2f * Mathf.Deg2Rad, 10);
        //newcode end
        UnityEditor.Handles.DrawWireArc(self.transform.position, new Vector3(0,0,1), from.normalized, 45f, 10f); //from -> start
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
            Gizmos.color = new Color(0.5f, 0.55f, 0.05f, 1f);
            Gizmos.DrawLine(this.transform.position, this.transform.position + guard.wallcheck*new Vector3(facing.x,facing.y,0).normalized);
            Vector3 checkLeft = Vector3.RotateTowards(facing, outTan, 90f * Mathf.Deg2Rad,guard.wallcheck); //-facing -> new vector(0,1,0)
            Gizmos.DrawLine(this.transform.position, this.transform.position + checkLeft.normalized* guard.wallcheck);
            Vector3 checkBehind = Vector3.RotateTowards(facing, -facing, 180f * Mathf.Deg2Rad, guard.wallcheck);
            Gizmos.DrawLine(this.transform.position, this.transform.position + checkBehind.normalized * guard.wallcheck);
            Vector3 checkRight = Vector3.RotateTowards(facing, new Vector3(0, 1, 0), -90f * Mathf.Deg2Rad, guard.wallcheck); //-facing -> new vector(0,1,0)
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
