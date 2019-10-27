using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsScript : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string inputsound;
    bool playerisMoving = false;
    public float walkingSpeed;
    private void Update()
    {
        if (Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Vertical") <= -0.01f || Input.GetAxis("Horizontal") >= 0.01f || Input.GetAxis("Horizontal") <= -0.01f)
        {
            playerisMoving = true;
        }else if(Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0){
            playerisMoving = false; 
        }
    }
    void CallFootsteps()
    {
        if (playerisMoving)
        {
            FMODUnity.RuntimeManager.PlayOneShot(inputsound);
        }
    }
    private void Start()
    {
        InvokeRepeating("CallFootsteps", 0, walkingSpeed);
    }
    private void OnDisable()
    {
        playerisMoving = false;
    }
}
