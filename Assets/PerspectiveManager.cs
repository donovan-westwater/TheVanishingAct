using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveManager : MonoBehaviour
{
    public GameObject tunnel;
    bool isOn = false;
    // Start is called before the first frame update
    void Start()
    {
        tunnel = GameObject.Find("SpatialTunnel").gameObject;
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
        if (Input.GetKeyDown(KeyCode.Space) && !isOn)
        {
            isOn = true;
            tunnel.SetActive(true);
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
            tunnel.SetActive(false);
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
}
