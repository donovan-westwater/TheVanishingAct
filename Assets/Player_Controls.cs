﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controls : MonoBehaviour
{
    public float speed;
    public GameObject aim_sprite;
    public  string[] inventory = new string[3];
    private Rigidbody2D rb2d;
    private Vector3 movement = new Vector3(0,0,0);
    private float vertical = 0f;
    private float horizontal = 0f;
    private float angle;
    private Vector3 baseDir = new Vector3(1, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        aim_sprite = gameObject.transform.GetChild(0).gameObject;
        Vector3 startVect = new Vector3(aim_sprite.transform.position.x -transform.position.x, aim_sprite.transform.position.y-transform.position.y).normalized;
        angle = Mathf.Acos(Vector3.Dot(startVect, baseDir) / (startVect.magnitude * baseDir.magnitude));
        speed = 10f;
        rb2d = GetComponent<Rigidbody2D>();
        
    }
    public Vector3 grabSpellAim()
    {
        return new Vector3(aim_sprite.transform.position.x - transform.position.x, aim_sprite.transform.position.y - transform.position.y);
    }
    //adds an item to inventory
    public void addItem(string name,int index)
    {
        inventory[index] = name;
    }
    //Removes an item from the players inventory
    public void removeItem(string name)
    {
        for(int n = 0; n < inventory.Length;n++)
        {
            if (inventory[n].Equals(name))
            {
                inventory[n] = null;
            }
        }
    }
    // Update is called once per frame
    //Change to be based on force
    void Update()
    {
        //Movment controls
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 tempVect = new Vector3(h, v, 0);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        if (!Input.anyKey)
        {
            tempVect = new Vector3(0,0,0);
        }
        rb2d.MovePosition(rb2d.transform.position + tempVect);
        //Shooting / aiming
        Vector3 mouseVect = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y);
        Vector3 currentDir = new Vector3(aim_sprite.transform.position.x-transform.position.x, aim_sprite.transform.position.y - transform.position.y);
        float angleTemp = Mathf.Acos(Vector3.Dot(currentDir, mouseVect) / (currentDir.magnitude * mouseVect.magnitude));
        if(Vector3.Cross(mouseVect.normalized,currentDir.normalized).z < 0 && angleTemp > Mathf.Abs(0.01f))
        {
            angle -= 0.1f;
            aim_sprite.transform.RotateAround(transform.position, new Vector3(0, 0, 1), 0.1f); //-0.1f
        }
        else if(Vector3.Cross(mouseVect.normalized, currentDir.normalized).z > 0 && angleTemp > Mathf.Abs(0.01f)) 
        {
            angle += 0.1f;
            aim_sprite.transform.RotateAround(transform.position, new Vector3(0, 0, 1), -0.1f); //-0.1f
        }
        print("Angle: " + angleTemp);
        //print("Cross: " + Vector3.Cross(mouseVect.normalized, currentDir.normalized).z);
        Debug.DrawLine(aim_sprite.transform.position, transform.position,Color.blue);
        Debug.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position, Color.blue);

        //print(aim_sprite.transform.position.x - transform.position.x);
        //aim_sprite.transform.RotateAround(transform.position, new Vector3(0, 0, 1), angleTemp);
    }
    public string[] getInventory()
    {
        return inventory;
    }
}
