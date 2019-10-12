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
            player.GetComponent<Player_Controls>().setMana(0);
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
            player.GetComponent<Player_Controls>().setMana(tempMana);
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
    }
    public bool[] shiftActive()
    {
        return whatsOn;
    }
}
