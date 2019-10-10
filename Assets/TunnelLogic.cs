using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelLogic : MonoBehaviour
{
    GameObject player;
    Vector2 tunnelDir;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").gameObject;
        tunnelDir = player.GetComponent<Player_Controls>().grabSpellAim();
        this.transform.position = player.transform.position;
        //Vector2 dir = player.GetComponent<Player_Controls>().grabSpellAim();
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //this.transform.Rotate(new Vector3(0,0,1),angle);
        
    }
    //Angle gets out of sync when crossing to other side since only the shortest angle is being looked at!
    private void OnEnable()
    {  
        
       
        //get cross product to determine turn direction and get magitude of angle to turn to correct place
        
       // print("Start: "+transform.position);
        player = GameObject.Find("Player").gameObject;
       // player.GetComponent<Collider2D>().enabled = false;
        this.transform.position = player.transform.position;
        Vector2 dir = player.GetComponent<Player_Controls>().grabSpellAim();
        float angle = Vector2.Angle(tunnelDir, dir);
        tunnelDir = dir;
        if (Vector3.Cross(dir.normalized, tunnelDir.normalized).z < 0 )
        {
            angle =  Vector2.Angle(tunnelDir, dir);
            //0.1f
        }
        else if (Vector3.Cross(dir.normalized, tunnelDir.normalized).z > 0)
        {
             angle= -Vector2.Angle(tunnelDir, dir);
            //-0.1f
        }
        this.transform.Rotate(new Vector3(0, 0, 1), angle, Space.World);
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //this.transform.RotateAround(player.transform.position, new Vector3(0, 0, 1), angle);
        //player.GetComponent<Collider2D>().enabled = true;
        //print("End: " + transform.position);


    }
    // Update is called once per frame
    void Update()
    {
       // player = GameObject.Find("Player").gameObject;
        
        //this.transform.Rotate(new Vector3(0, 0, 1), 0.1f,Space.World); this one works
    }
}
