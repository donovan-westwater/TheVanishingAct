using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alarm : MonoBehaviour
{
    public GameObject[] attachedTriggers = new GameObject[4];
    public float alertRadis;
    public float triggerRadis;
    Vector2 lastSeen;
    bool alarmTriggered = false;
    float timer = 0;
    float waitTime = 4f;
    //bool sentAlert = false;

    void Start()
    {
        // base.Start();
        lastSeen = GameObject.Find("Player").transform.position;
        alertRadis = 20f;
        triggerRadis = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("Player");
        //Change to a raycast system! [move this over to camera script]
        //have a compare tag system for the interactibles, all should have a triggered setting.
        //use OOD for interactibles that trigger the alarm!!
        if (Vector2.Distance(player.transform.position, this.transform.position) < triggerRadis)
        {
            lastSeen = player.transform.position;
            alarmTriggered = true;
        }
        if (alarmTriggered)
        {
            timer += 1 * Time.deltaTime;
            alertGuards();
            if(timer > waitTime)
            {
                alarmTriggered = false;
                timer = 0;
            }
        }
    }
    bool alertGuards()
    {
        bool ret = false;

        GameObject gameout = new GameObject();
        GameObject[] guards = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject g in guards)
        {
            if (g.transform != this.transform && Vector2.Distance(g.transform.position, this.transform.position) < alertRadis)
            {
                ret = true;
                g.GetComponent<BasicAi>().setAlert(true);
                gameout.transform.position = lastSeen;
                g.GetComponent<BasicAi>().setWhereToSearch(gameout.transform);
            }
        }

        return ret;
    }
}
