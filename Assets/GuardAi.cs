using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GuardAi : BasicAi
{
    // Start is called before the first frame update
    //public Transform playerPosition;

    //private Seeker seeker;
    public Transform guardLoc;
    public float wallcheck = 5f;
    enum basicStates
    {
        guard = 0,
        chase = 1,
        attack = 2,
        moveBack = 3,
        alerted = 4
    }
    enum directions
    {
        north = 0,
        south = 1,
        east = 2,
        west = 3
    }
    basicStates curState = basicStates.guard;
    directions curdirction;
    //public Path path;

    //public float speed = 2;

    //public float nextWaypointDistance = 0; //3

  // private int currentWaypoint = 0;
    private int timer = 0;
    //public float repathRate = 0.5f;
  //  private float lastRepath = float.NegativeInfinity;

    //public bool reachedEndOfPath;
    new void Start()
    {
        base.Start();
        curdirction = currentDirection();
    }
    private new void  FixedUpdate()
    {
        base.FixedUpdate();
        timer += 1;
    }
    // Update is called once per frame
   void Update()
    {
        if (Input.GetKey(KeyCode.K)) return;
        Vector3 aimDir = gameObject.transform.GetChild(0).position;
        aimDir.z = transform.position.z;
        RaycastHit2D rayDir = Physics2D.Raycast(transform.position, aimDir, wallcheck);
        curdirction = currentDirection();
        if (base.getAlert())
        {
            curState = basicStates.alerted;
        }
        if(curState == basicStates.guard)
        {
            if (Vector2.Distance(guardLoc.position, this.transform.position) > 1f) curState = basicStates.moveBack;
            switch (curdirction)
            {
                case directions.east:
                  
                    if (rayDir && rayDir.collider.CompareTag("Wall")) updateFacing(90);
                    else
                    {
                        if (base.canSeePlayer()) curState = basicStates.chase;
                    }
                    break;
                case directions.west:
                    if (rayDir && rayDir.collider.CompareTag("Wall")) updateFacing(90);
                    else
                    {
                        if (base.canSeePlayer()) curState = basicStates.chase;
                    }
                    break;
                case directions.north:
                    if (rayDir && rayDir.collider.CompareTag("Wall")) updateFacing(90);
                    else
                    {
                        if (base.canSeePlayer()) curState = basicStates.chase;
                    }
                    break;
                case directions.south:
                    if (rayDir && rayDir.collider.CompareTag("Wall")) updateFacing(90);
                    else
                    {
                        if (base.canSeePlayer()) curState = basicStates.chase;
                    }
                    break;
            }
            if (timer % 50 == 0) updateFacing(90);
        }
        else if(curState == basicStates.chase)
        {
            base.moveAI(playerPosition);
            if (!canSeePlayer())
            {
                curState = basicStates.guard;
            }
            base.attack();
        }
        else if(curState == basicStates.moveBack)
        {
            moveAI(guardLoc);
            if (Vector2.Distance(guardLoc.position, this.transform.position) < 1f)
            {
                curState = basicStates.guard;
                //Snap back into cardinal directions here
            }
        }else if(curState == basicStates.alerted)
        {
            moveAI(base.getWhereToSearch());
            if (canSeePlayer())
            {
                curState = basicStates.chase;
                base.setAlert(false);
            }
            if(Vector2.Distance(base.getWhereToSearch(),this.transform.position) < 1f)
            {
                base.alertTimer += 1 * Time.deltaTime;
                if(alertTimer >= base.alertWait)
                {
                    base.alertTimer = 0;
                    base.setAlert(false);
                    curState = basicStates.moveBack;
                }
            }
        }
        //Attack State goes here! (if close to player attack) [No longer its own state]
    }
    
    directions currentDirection()
    {
        if(Vector2.Angle(base.getFacing(), new Vector2(0,1)) < 45f)
        {
            return directions.north;
        }
        else if (Vector2.Angle(base.getFacing(), new Vector2(1, 0)) < 45f)
        {
            return directions.west;
        }else if (Vector2.Angle(base.getFacing(), new Vector2(0, -1)) < 45f)
        {
            return directions.south;
        }
        else
        {
            return directions.east;
        }
        
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
       
    }
}
