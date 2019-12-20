using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowardAi : BasicAi
{
    // Start is called before the first frame update
    public float alertRadis;
    public float searchRadis;
    //Transform lastSeen;
    Vector2 lastSeen;
    Transform home;
    bool sentAlert = false;
    bool searchDone = false;
    Transform currGuard;
    enum basicStates
    {
       watch = 1,
       flee = 2,
       alert = 3
    }
    basicStates curState = basicStates.watch;
    new void Start()
    {
        base.Start();
        lastSeen = GameObject.Find("Player").transform.position;
        GameObject gout = new GameObject();
        gout.transform.position = transform.position;
        home = gout.transform;
        alertRadis = 20f;
        searchRadis = 60f;
    }

    // Update is called once per frame
     void Update()
    {
        
        GameObject player = GameObject.Find("Player");
        if (Input.GetKey(KeyCode.L) && player.GetComponent<Player_Controls>().getMark() == this.gameObject) return;
        if (curState == basicStates.watch)
        {
            if (canSeePlayer()||base.getAlert())
            {
                lastSeen = player.transform.position;
                if (base.getAlert()) lastSeen = base.getWhereToSearch();
                 curState = basicStates.flee;
            }
            if(Vector2.Distance(transform.position,home.position) > 1)
            {
                moveAI(home);
            }
        }else if(curState == basicStates.flee)
        {
            /*
            GameObject flee = new GameObject();
            Vector2 curPos = this.transform.position;
            Vector2 fleeDir = player.GetComponent<Player_Controls>().grabSpellAim();
            flee.transform.position = new Vector3(curPos.x+fleeDir.x,curPos.y+fleeDir.y,this.transform.position.z);
            moveAI(flee.transform);
            */
            GameObject[] guards = GameObject.FindGameObjectsWithTag("enemy");
            
            //int pickIndex = Random.Range(0, guards.Length - 1);
            if(!searchDone)
            {
                foreach (GameObject g in guards)
                {
                    if (g.transform != this.transform && Vector2.Distance(g.transform.position, this.transform.position) < searchRadis)
                    {
                        currGuard = g.transform;
                        searchDone = true;
                        break;
                    }
                }
                if(currGuard == null)
                {
                    int pickIndex = Random.Range(0, guards.Length - 1);
                    currGuard = guards[pickIndex].transform;
                }
            }
            moveAI(currGuard);
            if (canSeeGuards())
            {
                searchDone = false;
                curState = basicStates.alert;
            }
            
        }
        else if (curState == basicStates.alert)
        {
            bool done = alertGuards();
            if (done)
            {
                base.setAlert(false);
                curState = basicStates.watch;
            }
        }
    }
    bool canSeeGuards()
    {
        
        Vector3 facingDir = base.getFacing();
        facingDir.z = transform.position.z;
        GameObject[] guards = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject g in guards)
        {
            RaycastHit2D sightLine = Physics2D.Raycast(transform.position, g.transform.position - transform.position, 2f);
            if (!sightLine)
            {
                //print("I cant see player");
                continue;
            }
            //print(Vector2.Angle(facingDir, playerPosition.position));
            if (Vector2.Angle(facingDir.normalized, (g.transform.position - this.transform.position).normalized) < 45f && sightLine.collider.CompareTag("enemy"))
            {
                //print("I CAN SEE YOU, YOU LITTLE MAGE");
                return true;
            }
            
        }
        return false;
    }
    //returns a bool to see if the guards have been alerted or not
    bool alertGuards()
    {
        bool ret = false;

       // GameObject gameout = new GameObject();
        GameObject[] guards = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject g in guards)
            {
                if (g.transform != this.transform && Vector2.Distance(g.transform.position, this.transform.position) < alertRadis)
                {
                ret = true;
                g.GetComponent<BasicAi>().setAlert(true);
               // gameout.transform.position = lastSeen;
                g.GetComponent<BasicAi>().setWhereToSearch(lastSeen);
                }
            }

        return ret;
    }
}
