using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tripwire : baseTrigger
{
    float timer = 0;
    float waitTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTriggerd)
        {
            timer += 1 * Time.deltaTime;
            if(timer > waitTime)
            {
                timer = 0;
                hasTriggerd = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") == true){
            hasTriggerd = true;
            whereTriggered = col.gameObject.transform.position;
        }
    }
    public override bool checkTrigger()
    {
        return hasTriggerd;
    }
    
}
