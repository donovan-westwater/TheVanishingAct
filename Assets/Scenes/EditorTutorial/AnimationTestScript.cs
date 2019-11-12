using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTestScript : MonoBehaviour
{
    public Animator fadeAnimation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            fadeAnimation.SetBool("Fade", true);
        }
        else
        {
            fadeAnimation.SetBool("Fade", false);
        }
    }
}
