using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSensor : baseTrigger
{
    public GameObject objectStored;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(objectStored != null && objectStored.activeSelf == false)
        {
            hasTriggerd = true;
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
