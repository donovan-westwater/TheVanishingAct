using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alarm : MonoBehaviour
{
    public float alertRadis;
    public float triggerRadis;
    Transform lastSeen;
    //bool sentAlert = false;
    
    new void Start()
    {
       // base.Start();
        lastSeen = GameObject.Find("Player").transform;
        alertRadis = 20f;
        triggerRadis = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("Player");
        if (Vector2.Distance(player.transform.position, this.transform.position) < triggerRadis)
        {
            lastSeen = player.transform;
        }
    }
    void alertGuards()
    {
        GameObject[] guards = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject g in guards)
        {
            if (Vector2.Distance(g.transform.position, this.transform.position) < alertRadis)
            {

            }
        }


    }
}
