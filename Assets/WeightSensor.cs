using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSensor : baseTrigger
{
    public GameObject objectStored;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(objectStored != null && objectStored.activeSelf == false)
        {
            hasTriggerd = true;
            whereTriggered = player.transform.position;
        }
        else
        {
            hasTriggerd = false;
        }

    }
    public override bool checkTrigger()
    {
        return hasTriggerd;
    }
}
