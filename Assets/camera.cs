using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : baseTrigger
{
    Vector2 facing;
    Transform playerPosition;
    // Start is called before the first frame update
    void Start()
    {
        facing = gameObject.transform.GetChild(0).transform.position - this.transform.position;
        playerPosition = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        hasTriggerd = checkTrigger();
        if (hasTriggerd)
        {
            whereTriggered = playerPosition.transform.position;
        }
    }
    
    public override bool checkTrigger()
    {
        
        //Vector3 facingDir = gameObject.transform.GetChild(0).transform.position - this.transform.position;
        Vector3 facingDir = this.facing;
        facingDir.z = transform.position.z;
        //Debug.DrawLine(this.transform.position, this.transform.position + facingDir, Color.green);
        //Currently will see through walls. Going to switch to a raycast system later!!!!!
        RaycastHit2D sightLine = Physics2D.Raycast(transform.position, playerPosition.position - transform.position, 10f);
       // Debug.DrawLine(this.transform.position, playerPosition.position, Color.green);
        if (!sightLine)
        {
            print("I cant see player");
            return false;
        }
        print(Vector2.Angle(facingDir, playerPosition.position));
        if (Vector2.Angle(facingDir.normalized, (playerPosition.position - this.transform.position).normalized) < 35f && sightLine.collider.name.Equals("Player"))
        {
            print("I CAN SEE YOU, YOU LITTLE MAGE");
            return true;
        }
        return false;
    }
    
}
