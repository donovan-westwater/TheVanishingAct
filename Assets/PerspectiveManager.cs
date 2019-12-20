using System.Collections;
using System.Collections.Generic;
using UnityEngine;
struct ObjectState
{
    public Vector2 pos;
    public Vector2 facing;
}
//Time spell has issues with the rotation of the of the facing, indictor starts to go wildly off base
//Make feedback for the spell, even if its just a gizmo
public class PerspectiveManager : MonoBehaviour
{
    private GameObject player;
    public GameObject tunnel;
    bool isOn = false;
    bool[] whatsOn = new bool[3];
    int tempMana;
    Queue<ObjectState> recordQ = new Queue<ObjectState>();
    int timeRecordMax = 100; //50 calls per second so records 500/50 == 10 sec default
    int frametimer = 0;
    int timeindx = 0;
    ObjectState[] timeReel;
    GameObject curTar;
    Vector2 watch;
    //bool isTraveling = false;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            whatsOn[i] = false;
        }
        player = GameObject.Find("Player").gameObject;
        tunnel = GameObject.Find("SpatialTunnel").gameObject;
        tempMana = player.GetComponent<Player_Controls>().getMana();
        tunnel.SetActive(false);
        GameObject[] gameObjectArray = Resources.FindObjectsOfTypeAll<GameObject>();
        // GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject go in gameObjectArray)
        {
            if (go.CompareTag("Wall") || go.CompareTag("enemy") || go.CompareTag("Glass"))
            {
                go.SetActive(true);
            }
        }
    }
    private void FixedUpdate()
    {

        if (player.GetComponent<Player_Controls>().hasMarked())
        {
            if (curTar != player.GetComponent<Player_Controls>().getMark())
            {
                recordQ = new Queue<ObjectState>();
                curTar = player.GetComponent<Player_Controls>().getMark();
                frametimer = 0;
            }
            if (!(Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L)))
            {
                ObjectState enqueState = new ObjectState();
                enqueState.pos = curTar.transform.position;
                //GameObject mark = player.GetComponent<Player_Controls>().getMark();
                if (curTar.CompareTag("enemy"))
                {
                    enqueState.facing = curTar.GetComponent<BasicAi>().getAim();
                }
                if (timeRecordMax < frametimer)
                {
                    recordQ.Dequeue();
                    frametimer -= 1;
                }
                recordQ.Enqueue(enqueState);
                frametimer += 1;
            }
            //Controls for scrolling through
            //isTraveling = false;
            if ((Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L)) && recordQ.Count > 0)
            {
                if(recordQ.Count > 0) timeReel = recordQ.ToArray();
                timeindx = timeReel.Length - 1;
            }
            if (Input.GetKey(KeyCode.K))
            {

                //loadState here
                watch = curTar.GetComponent<BasicAi>().getAim();
                loadState(timeReel[timeindx]);
                watch = curTar.GetComponent<BasicAi>().getAim();
                if (timeindx + 1 < timeReel.Length) timeindx += 1;
            }
            else if (Input.GetKey(KeyCode.L))
            {
                watch = curTar.GetComponent<BasicAi>().getAim();
                //loadState here
                loadState(timeReel[timeindx]);
                if (timeindx - 1 > 0) timeindx -= 1;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && !isOn && player.GetComponent<Player_Controls>().getMana() >= 2)
        {
            isOn = true;
            whatsOn[0] = true;
            tunnel.SetActive(true);
            tempMana = player.GetComponent<Player_Controls>().getMana();
            player.GetComponent<Player_Controls>().setMana(tempMana - 2);
           // player.GetComponent<Player_Controls>().setMana(0);
            GameObject[] gameObjectArray = Resources.FindObjectsOfTypeAll<GameObject>();
            // GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject go in gameObjectArray)
            {
                if (go.CompareTag("Wall") || go.CompareTag("enemy")||go.CompareTag("Glass"))
                {
                    go.SetActive(false);
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.Space) && isOn)
        {
            isOn = false;
            whatsOn[0] = false;
            tunnel.SetActive(false);
            //player.GetComponent<Player_Controls>().setMana(tempMana);
            GameObject[] gameObjectArray = Resources.FindObjectsOfTypeAll<GameObject>();
           // GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject go in gameObjectArray)
            {
                if (go.CompareTag("Wall") || go.CompareTag("enemy")||go.CompareTag("Glass"))
                {
                    go.SetActive(true);
                }
            }
            
        }
        
        //Thought space activation [Need to expand to interactibles as well in terms of color changes] [check if gameobject is a prefab before changing color!]
        /*
        else if (Input.GetKeyDown(KeyCode.J) && !isOn && !whatsOn[1])
        {
            isOn = true;
            whatsOn[1] = true;
  
            player.GetComponent<Player_Controls>().setMana(0);
            GameObject background = GameObject.Find("Background");
            background.GetComponent<SpriteRenderer>().color = Color.white;
            GameObject[] gameObjectArray = Resources.FindObjectsOfTypeAll<GameObject>();
            // GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject go in gameObjectArray)
            {
                //if(go.CompareTag("Traversible")) go.GetComponent<SpriteRenderer>().color = Color.white;
                if (go.CompareTag("Wall") || go.CompareTag("Glass"))
                {
                    go.GetComponent<SpriteRenderer>().color = Color.white;
                    go.GetComponent<Wall>().setThoughtSpace(true);
                }
            }
            //Call player function here
            player.GetComponent<Player_Controls>().setThoughtSpace(true);
        }
        else if (Input.GetKeyDown(KeyCode.J) && isOn && whatsOn[1])
        {
            isOn = false;
            whatsOn[1] = false;

            player.GetComponent<Player_Controls>().setMana(3);
            //GameObject background = GameObject.Find("Background");
            //background.GetComponent<SpriteRenderer>().color = new Color(255, 154, 6, 255);
            GameObject[] gameObjectArray = Resources.FindObjectsOfTypeAll<GameObject>();
            // GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject go in gameObjectArray)
            {
                go.GetComponent<Wall>().setThoughtSpace(false);
                if (go.CompareTag("Wall") || go.CompareTag("Glass"))
                {
                    //go.GetComponent<SpriteRenderer>().color = Color.black;
                    go.GetComponent<Wall>().restoreColor();
                }
               // if (go.CompareTag("Traversible")) go.GetComponent<SpriteRenderer>().color = new Color(147,87,20,255); //broekn color
                /*
                else if (go.CompareTag("Glass"))
                {
                    //go.GetComponent<SpriteRenderer>().color = new Color(4,250,235,255);
                }
               
            }
            //Call player function here
            player.GetComponent<Player_Controls>().setThoughtSpace(false);
        }
        */

    }
    void loadState(ObjectState state)
    {
        if (curTar == null) return;
        curTar.transform.position = new Vector3(state.pos.x, state.pos.y, curTar.transform.position.z);
        if (state.facing != null)
        {
            GameObject aimSprite = curTar.transform.GetChild(0).gameObject;
            Vector2 curFace = curTar.GetComponent<BasicAi>().getAim();
            if (Vector3.Cross(curFace.normalized, state.facing).z < 0)
            {
                aimSprite.transform.RotateAround(curTar.transform.position, new Vector3(0, 0, 1), -Vector2.Angle(curFace, state.facing)); //-0.1f
            }
            else if (Vector3.Cross(curFace, state.facing).z > 0)
            {

                aimSprite.transform.RotateAround(curTar.transform.position, new Vector3(0, 0, 1), Vector2.Angle(curFace, state.facing));
            }
        }
    }
    public bool[] shiftActive()
    {
        return whatsOn;
    }
}
