﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valuables : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");    
    }

    // Update is called once per frame
    void Update()
    {
               
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("pellet"))
        {
            print("object: I HAVE BEEN HIT!");
        }
    }
}