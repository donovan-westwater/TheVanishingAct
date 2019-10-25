using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private Vector3 warpDir;
    private Quaternion ogRot;
    private Vector3 ogPos;
    public GameObject wall;
    private GameObject player;

    //All of this warp code should go into its own script later!
    // Start is called before the first frame update
    void Start()
    {
        /*
        player = GameObject.Find("Player");
        ogPos = transform.position;
        warpDir = player.GetComponent<Player_Controls>().grabSpellAim();
        ogRot = transform.localRotation;
        */
    }

    // Update is called once per frame
    void Update()
    {
        /*
        warpDir = player.GetComponent<Player_Controls>().grabSpellAim();
        Vector3 warpAxis = Vector2.Perpendicular(new Vector2(warpDir.x, warpDir.y));

        if (Input.GetKey(KeyCode.N))
        {
            transform.Rotate(warpAxis, 1f,Space.World);
            //transform.Rotate(0,1f,0,Space.Self);
           if(transform.rotation.eulerAngles.y > 85 && transform.rotation.eulerAngles.y < 95)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -50);
                wall.GetComponent<BoxCollider2D>().enabled = false;
            }
            else {
                transform.position = new Vector3(transform.position.x,transform.position.y,ogPos.z);
                wall.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        else
        {
            transform.rotation = ogRot;
        }
        
        if (Input.GetKey(KeyCode.M))
        {
            transform.position = new Vector3(transform.position.x - warpDir.normalized.x*0.01f, transform.position.y - warpDir.normalized.y * 0.01f, transform.position.z);
            //transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = ogPos;
        }
        */
    }
    //Bullets need to die when they colldie
    private void OnTriggerEnter2D(Collider2D col)
    {
        /*
        print("Will it hit?");
        if (col.gameObject.CompareTag("pellet"))
        {
            print("Wall: I HAVE BEEN HIT!");
        }
        */
    }
}
