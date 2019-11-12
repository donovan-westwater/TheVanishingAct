using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private Vector3 warpDir;
    private Quaternion ogRot;
    private Vector3 ogPos;
    public GameObject wall;
    private GameObject player;
    private Color main;
    private bool thoughtSpace = false;
    private double vanishTimer = 0;
    //All of this warp code should go into its own script later!
    // Start is called before the first frame update
    void Start()
    {
        //main = this.GetComponent<SpriteRenderer>().material.color;
        //if (this.CompareTag("Glass")) this.GetComponent<Renderer>().color = Color.cyan;
        //else this.GetComponent<SpriteRenderer>().color = Color.black;
        main = this.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        vanishTimer -= 1*Time.deltaTime;
        if ((thoughtSpace && vanishTimer <= 0))
        {
            //this.GetComponent<SpriteRenderer>().color = Color.white;
            this.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (Input.GetMouseButton(1))
        {
            this.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (Input.GetMouseButton(0)) restoreColor();
        if(vanishTimer < 0)
        {
            vanishTimer = 0;
        }

    }
    public void setThoughtSpace(bool truth)
    {
        thoughtSpace = truth;
    }
    public void addToTimer()
    {
        vanishTimer += 15;
    }
    public void restoreColor()
    {
        //if (this.CompareTag("Glass")) this.GetComponent<SpriteRenderer>().color = Color.cyan;
        //else this.GetComponent<SpriteRenderer>().color = Color.black;
        this.GetComponent<SpriteRenderer>().color = main;
    }
}
