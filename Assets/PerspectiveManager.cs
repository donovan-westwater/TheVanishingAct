using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveManager : MonoBehaviour
{
    private GameObject player;
    public GameObject tunnel;
    bool isOn = false;
    bool[] whatsOn = new bool[3];
    int tempMana;
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
                if(go.CompareTag("Traversible")) go.GetComponent<SpriteRenderer>().color = Color.white;
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
            GameObject background = GameObject.Find("Background");
            background.GetComponent<SpriteRenderer>().color = new Color(255, 154, 6, 255);
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
                if (go.CompareTag("Traversible")) go.GetComponent<SpriteRenderer>().color = new Color(147,87,20,255); //broekn color
                /*
                else if (go.CompareTag("Glass"))
                {
                    //go.GetComponent<SpriteRenderer>().color = new Color(4,250,235,255);
                }
                */
            }
            //Call player function here
            player.GetComponent<Player_Controls>().setThoughtSpace(false);
        }
    }
    public bool[] shiftActive()
    {
        return whatsOn;
    }
}
