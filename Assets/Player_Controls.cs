using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controls : MonoBehaviour
{
    public float speed;
    public GameObject aim_sprite;
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
        Vector3 mouseVect = new Vector3(Input.mousePosition.x - transform.position.x, Input.mousePosition.y - transform.position.y);
        Vector3 currentDir = new Vector3(aim_sprite.transform.position.x - transform.position.x, aim_sprite.transform.position.y - transform.position.y);
        float angleTemp = Mathf.Acos(Vector3.Dot(currentDir, mouseVect) / (currentDir.magnitude * mouseVect.magnitude));
        if(aim_sprite.transform.position.x - transform.position.x < 0 || aim_sprite.transform.position.y - transform.position.y < 0)
        {
            angle -= angleTemp;
            aim_sprite.transform.RotateAround(transform.position, new Vector3(0, 0, 1), -angleTemp);
        }
        else if(aim_sprite.transform.position.x - transform.position.x > 0 || aim_sprite.transform.position.y - transform.position.y > 0)
        {
            angle += angleTemp;
            aim_sprite.transform.RotateAround(transform.position, new Vector3(0, 0, 1), angleTemp);
        }
        //angle = Mathf.Acos(Vector3.Dot(mouseVect, baseDir) / (mouseVect.magnitude * baseDir.magnitude));
        //print(angle);
       // print("MouseVect: " + mouseVect);
        
        print(aim_sprite.transform.position.x - transform.position.x);
        //aim_sprite.transform.RotateAround(transform.position, new Vector3(0, 0, 1), angleTemp);
    }
}
