using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PatrolAi : BasicAi
{
    //public Transform playerPosition;
    //Patrol system
    //PatrolAi self;
    public Transform[] targets;
    int curIndex = 0;
    int start = 0;
    int end;
    int direction = 1;
    //private Seeker seeker;
    enum basicStates
    {
        patrol = 0,
        chase = 1,
        attack = 2,
        alerted = 3
    }
    basicStates curState = basicStates.patrol;
    //public Path path;

    //public float speed = 2;

    //public float nextWaypointDistance = 0; //3

    //private int currentWaypoint = 0;

    // public float repathRate = 0.5f;
    //private float lastRepath = float.NegativeInfinity;

    //public bool reachedEndOfPath;

    public new void Start()
    {

        base.Start();
        end = targets.Length - 1;
        // Start a new path to the playerPosition, call the the OnPathComplete function
        // when the path has been calculated (which may take a few frames depending on the complexity)
        //seeker.StartPath(transform.position, targets[0].position, OnPathComplete);
    }
    /*
    public void OnPathComplete(Path p)
    {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        // Path pooling. To avoid unnecessary allocations paths are reference counted.
        // Calling Claim will increase the reference count by 1 and Release will reduce
        // it by one, when it reaches zero the path will be pooled and then it may be used
        // by other scripts. The ABPath.Construct and Seeker.StartPath methods will
        // take a path from the pool if possible. See also the documentation page about path pooling.
        p.Claim(this);
        if (!p.error)
        {
            if (path != null) path.Release(this);
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
        else
        {
            p.Release(this);
        }
    }
    */
    public void Update()
    {


        //print("Distance to target: " + Vector2.Distance(this.transform.position, targets[curIndex].position));
        //Patrol system needs work!
        if (base.getAlert())
        {
            curState = basicStates.alerted;
        }
        //Switch to seeing system with a wait timer before ignoring the player
        if (curState == basicStates.patrol)
        {
          
            base.moveAI(targets[curIndex]);
            if (Vector2.Distance(this.transform.position, targets[curIndex].position) < 2)
            {
                curIndex += direction;
                print("Current index: " + curIndex);

            }
            if ((curIndex > end && direction > 0) || (curIndex < end && direction < 0))
            {
                curIndex = start;
                start = end;
                end = curIndex;
                curIndex = start - direction;
                direction = -direction;
            }
            if (base.canSeePlayer())
            {
                curState = basicStates.chase;
            }
            /*
            if (Vector2.Distance(playerPosition.position, this.transform.position) < 10)
            {
                curState = basicStates.chase;
            }
            */
            //moveAI(targets[index]);

        }
        else if (curState == basicStates.chase)
        {
            base.moveAI(playerPosition);
            if (Vector2.Distance(playerPosition.position, this.transform.position) > 10)
            {
                curState = basicStates.patrol;
            }
            base.attack();
        }
        else if (curState == basicStates.alerted)
        {
            moveAI(base.getWhereToSearch());
            if (canSeePlayer())
            {
                curState = basicStates.chase;
                base.setAlert(false);
            }
            if (Vector2.Distance(base.getWhereToSearch(), this.transform.position) < 1)
            {
                base.alertTimer += 1 * Time.deltaTime;
                if (alertTimer >= base.alertWait)
                {
                    base.alertTimer = 0;
                    base.setAlert(false);
                    curState = basicStates.patrol;
                }
            }
            //Attack state goes here! [Attack will no longer be its own state]
            //moveAI(playerPosition);
        }
    }
        void FixedUpdate()
        {
            Bounds playerB;
            Bounds aiB;
            playerB = GameObject.Find("Player").GetComponent<Collider2D>().bounds;
            aiB = GetComponent<Collider2D>().bounds;

            AstarPath.active.UpdateGraphs(playerB);
            AstarPath.active.UpdateGraphs(aiB);
        }/*
    void moveAI(Transform target)
    {
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;

            // Start a new path to the playerPosition, call the the OnPathComplete function
            // when the path has been calculated (which may take a few frames depending on the complexity)
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
        if (path == null)
        {
            // We have no path to follow yet, so don't do anything
            return;
        }

        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        Debug.DrawLine(this.transform.position, path.vectorPath[currentWaypoint], Color.red);
        while (true)
        {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
            // print("Current Distance: " + distanceToWaypoint);
            //print("NextDistnace: " + nextWaypointDistance);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;

                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector2 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed * speedFactor;



        // If you are writing a 2D game you may want to remove the CharacterController and instead use e.g transform.Translate
        //transform.position += velocity * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime);
    }
    */
    
}
