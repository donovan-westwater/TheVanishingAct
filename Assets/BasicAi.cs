using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BasicAi : MonoBehaviour
{
    public Transform playerPosition;
    private float thoughtRadius = 15;
    private Seeker seeker;

    private bool playerVisible = true;

    public Path path;

    public float speed = 2;

    public float nextWaypointDistance = 2f; //3

    private int currentWaypoint = 0;

    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;

    public bool reachedEndOfPath;

    bool alerted = false;
    public float alertWait = 5f;
    public float alertTimer = 0;
    Vector2 whereToSearch; //was once a transform

    Vector2 facing;
    
    public void Start()
    {
        seeker = GetComponent<Seeker>();
        playerPosition = GameObject.Find("Player").transform;
        facing = gameObject.transform.GetChild(0).transform.position - this.transform.position;

        // Start a new path to the playerPosition, call the the OnPathComplete function
        // when the path has been calculated (which may take a few frames depending on the complexity)
        //seeker.StartPath(transform.position, targets[0].position, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        //Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

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
/*
    public void Update()
    {
        
        
       
    }
    */
    
   public void FixedUpdate()
    {
       // Bounds playerB;
        Bounds aiB;
       // playerB = GameObject.Find("Player").GetComponent<Collider2D>().bounds;
        aiB = GetComponent<Collider2D>().bounds;

        //AstarPath.active.UpdateGraphs(playerB);
        AstarPath.active.UpdateGraphs(aiB);
        //WIP thoughtspace code
        //Look into making objects visible by using a physics2d.CircleCastAll
        if (playerPosition.transform.gameObject.GetComponent<Player_Controls>().getThoughtSpace())
        {
            RaycastHit2D[] percievedItems = Physics2D.CircleCastAll(transform.position,thoughtRadius,facing,0.1f); //Checks for colliders around the ai
            foreach(RaycastHit2D ray in percievedItems)
            {
                GameObject go = ray.collider.gameObject;
                if(go.CompareTag("Wall"))
                {
                    go.GetComponent<SpriteRenderer>().color = Color.black;
                    go.GetComponent<Wall>().addToTimer();
                }
                else if (go.CompareTag("Glass"))
                {
                    go.GetComponent<SpriteRenderer>().color = Color.cyan;
                    go.GetComponent<Wall>().addToTimer();
                }
            }
            if(Vector2.Distance(transform.position,playerPosition.position) <= thoughtRadius)
            {
                Physics2D.IgnoreLayerCollision(0, 1, false);
            }
        }
    }
    public void moveAI(Transform target)
        
    {
        GameObject aimSprite = transform.GetChild(0).gameObject;
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
        //Rotates aim sprite to face direction of movment, currently a bit too slow
        if (Vector3.Cross(facing.normalized, dir).z < 0)
        {
            aimSprite.transform.RotateAround(transform.position, new Vector3(0, 0, 1), -Vector2.Angle(facing,dir)*Time.deltaTime); //-0.1f
        }
        else if (Vector3.Cross(facing.normalized, dir).z > 0)
        {

            aimSprite.transform.RotateAround(transform.position, new Vector3(0, 0, 1), Vector2.Angle(facing, dir)*Time.deltaTime);
        }
        facing = aimSprite.transform.position - this.transform.position;
        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed * speedFactor;



        // If you are writing a 2D game you may want to remove the CharacterController and instead use e.g transform.Translate
        //transform.position += velocity * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime);
    }
    public void moveAI(Vector2 target)

    {
        GameObject aimSprite = transform.GetChild(0).gameObject;
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;

            // Start a new path to the playerPosition, call the the OnPathComplete function
            // when the path has been calculated (which may take a few frames depending on the complexity)
            seeker.StartPath(transform.position, target, OnPathComplete);
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
        //Rotates aim sprite to face direction of movment, currently a bit too slow
        if (Vector3.Cross(facing.normalized, dir).z < 0)
        {
            aimSprite.transform.RotateAround(transform.position, new Vector3(0, 0, 1), -Vector2.Angle(facing, dir) * Time.deltaTime); //-0.1f
        }
        else if (Vector3.Cross(facing.normalized, dir).z > 0)
        {

            aimSprite.transform.RotateAround(transform.position, new Vector3(0, 0, 1), Vector2.Angle(facing, dir) * Time.deltaTime);
        }
        facing = aimSprite.transform.position - this.transform.position;
        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed * speedFactor;



        // If you are writing a 2D game you may want to remove the CharacterController and instead use e.g transform.Translate
        //transform.position += velocity * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime);
    }
    public bool canSeePlayer()
    {
        //Vector3 facingDir = gameObject.transform.GetChild(0).transform.position - this.transform.position;
        Vector3 facingDir = this.facing;
        facingDir.z = transform.position.z;
        //Debug.DrawLine(this.transform.position, this.transform.position + facingDir, Color.green);
        //Currently will see through walls. Going to switch to a raycast system later!!!!!
        RaycastHit2D sightLine = Physics2D.Raycast(transform.position, playerPosition.position-transform.position, 10f);
        Debug.DrawLine(this.transform.position, playerPosition.position, Color.green);
        if (!sightLine)
        {
            print("I cant see player");
            return false;
        }
        //print(Vector2.Angle(facingDir, playerPosition.position));
        if (playerVisible && Vector2.Angle(facingDir.normalized,(playerPosition.position - this.transform.position).normalized) < 45f && sightLine.collider.name.Equals("Player"))
        {
            print("I CAN SEE YOU, YOU LITTLE MAGE");
            return true;
        }
        return false;
    }
    public void updateFacing(float degrees)
    {
        //rotate around fuction called here, then update facing by recalculating aim sprite
        GameObject aim_sprite = this.gameObject.transform.GetChild(0).gameObject;
        aim_sprite.transform.RotateAround(transform.position, new Vector3(0, 0, 1), degrees);
        this.facing = gameObject.transform.GetChild(0).transform.position - this.transform.position;
    }
    public void setVisible(bool visiblity)
    {
        this.playerVisible = visiblity;
    }
    public Vector2 getFacing()
    {
        return this.facing;
    }
    public void setAlert(bool truth)
    {
        this.alerted = truth;
    }
    public bool getAlert()
    {
        return this.alerted;
    }
    public Vector2 getWhereToSearch()
    {
        return whereToSearch;
    }
    public void setWhereToSearch(Vector2 loc)
    {
        whereToSearch = loc;
    }
    public void attack()
    {
        GameObject player = GameObject.Find("Player");
        if(Vector2.Distance(playerPosition.position,this.transform.position) < 2)
        {
            player.SetActive(false);
        }
    }

}
    
