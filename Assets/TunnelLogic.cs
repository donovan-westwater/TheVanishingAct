using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelLogic : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").gameObject;
        this.transform.position = player.transform.position;
        //Vector2 dir = player.GetComponent<Player_Controls>().grabSpellAim();
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //this.transform.Rotate(new Vector3(0,0,1),angle);
        
    }
    private void OnEnable()
    {
        print(transform.position);
        player = GameObject.Find("Player").gameObject;
        //this.transform.position = player.transform.position;
        Vector2 dir = player.GetComponent<Player_Controls>().grabSpellAim();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        this.transform.Rotate(new Vector3(0,0,1),angle);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
