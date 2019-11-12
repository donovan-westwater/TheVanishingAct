using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[ExecuteInEditMode]
public class editModeTest : MonoBehaviour
{
    // Start is called before the first frame update
    public Material mat;
    public static Vector3 direction;
    public float speed = 1f;
    void Start()
    {
        direction = new Vector3(2, -1, 6); 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        mat.color = new Color(rotation.x/360f, rotation.y/360f, rotation.z/360f);
        transform.Rotate(direction * speed * Time.deltaTime);
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(.5f, .5f, .5f, .5f);
        Gizmos.DrawSphere(transform.position, speed);
    }
    [MenuItem("MyMenu/Rotate all objects %#&r",true)] //Symbols create a hotkey!
    static bool doSometingTest()
    {
        if (GameObject.FindGameObjectWithTag("enemy")) return true;
        return false;
    }
    [MenuItem("MyMenu/Rotate all objects %#&r", false)]
    static void DoSomething()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("enemy"))
        {
            g.transform.Rotate(new Vector3(1, 1, 1), 90);
        }
    }
}
