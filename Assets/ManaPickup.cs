using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPickup : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        int mana = player.GetComponent<Player_Controls>().getMana();
        if (col.CompareTag("Player") == true)
        {
            mana += 1;
            if(mana > 3)
            {
                mana = 3;
            }
            player.GetComponent<Player_Controls>().setMana(mana);
            this.gameObject.SetActive(false);
        }
    }
}
