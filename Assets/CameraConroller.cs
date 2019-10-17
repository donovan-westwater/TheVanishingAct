using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConroller : MonoBehaviour
{
    GameObject player;
    Vector2 dir = new Vector2(0, 0);
    Vector3 screenPoint;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        screenPoint = Camera.main.WorldToScreenPoint(player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        screenPoint = Camera.main.WorldToScreenPoint(player.transform.position);
        if (screenPoint.x > Camera.main.pixelWidth || screenPoint.x < 0 || screenPoint.y > Camera.main.pixelHeight || screenPoint.y < 0)
        {
            dir = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
            transform.position = new Vector3(transform.position.x + dir.x, transform.position.y + dir.y, transform.position.z);
          
        }
    }
}
