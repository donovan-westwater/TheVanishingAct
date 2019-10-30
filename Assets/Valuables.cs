using System.Collections;
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
        
        if (col.gameObject.CompareTag("Player"))
        {
            
             int curInvSize = col.gameObject.GetComponent<Player_Controls>().getInvSize() + 1;
            if (curInvSize < col.gameObject.GetComponent<Player_Controls>().inventory.Length)
            {
                col.gameObject.GetComponent<Player_Controls>().addItem("Object", curInvSize - 1);
                this.gameObject.SetActive(false);
            }
            else
            {
                print("Inventory Full!");
            }
        }
    }
}
