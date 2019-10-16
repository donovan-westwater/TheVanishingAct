using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class baseTrigger : MonoBehaviour
{
    public bool hasTriggerd = false;
    public Vector2 whereTriggered;
    
    public Vector2 getWhereTriggered()
    {
        return whereTriggered;
    }
    public bool getIfTriggered()
    {
        return hasTriggerd;
    }
    public abstract bool checkTrigger();
    
}
