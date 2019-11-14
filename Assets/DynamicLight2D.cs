using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLight2D : MonoBehaviour
{
    // Start is called before the first frame update
    public float lightRadius = 5f;
    public int rayAmount = 50; //How many rays sent out when raycasting?
    void Start()
    {
        
    }

    // Update is called once per frame
    //Raycast in a circle around the lightsource and create a mesh out of the vertices (points where ray hit collider)
    //Rays should be a fixed distance, this should create a radius of light with shadows. (will look jittery in motion without adjustments)
    void Update()
    {
        
    }
}
