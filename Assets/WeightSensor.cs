using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSensor : baseTrigger
{
    public GameObject objectStored;
    GameObject player;
    public float triggerTime = 3f;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTriggerd)
        {
            timer += 1 * Time.deltaTime;
            if (timer > triggerTime)
            {
                //timer = 0;
                hasTriggerd = false;
            }
        }
        if(objectStored != null && objectStored.activeSelf == false && timer == 0)
        {
            hasTriggerd = true;
            whereTriggered = player.transform.position;
        }
        

    }
    public override bool checkTrigger()
    {
        return hasTriggerd;
    }
}
