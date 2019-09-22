using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject player;
    public Image item;
    private string[] uiInventory;
    // Start is called before the first frame update
    void Start()
    {
        item = item.gameObject.GetComponent<Image>();
        player = GameObject.Find("Player");
        uiInventory = player.GetComponent<Player_Controls>().getInventory();
        item.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        uiInventory = player.GetComponent<Player_Controls>().getInventory();
        if(!uiInventory[0].Equals(""))
        {
            item.enabled = true;
        }
        else
        {
            item.enabled = false;
        }
    }
}
