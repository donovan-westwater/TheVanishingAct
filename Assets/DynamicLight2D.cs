using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLight2D : MonoBehaviour
{
    // Start is called before the first frame update
    public float lightRadius = 5f;
    public int rayAmount = 50; //How many rays sent out when raycasting?

    float angleStep;
    int vertAmount; 
    Vector3[] vertices;
    int[] triangles;
    Mesh lightMesh;
    MeshFilter meshFilter;

    Vector2 startDir = new Vector2(0, 1);
    void Start()
    {
        vertAmount = rayAmount + 1;
        angleStep = 360f / rayAmount; 
        triangles = new int[(vertAmount-1)*3];
        vertices = new Vector3[vertAmount];
        meshFilter = this.GetComponent<MeshFilter>();
        lightMesh = new Mesh();
        lightMesh.Clear();
        meshFilter.mesh = lightMesh;
    }

    // Update is called once per frame
    //Raycast in a circle around the lightsource and create a mesh out of the vertices (points where ray hit collider)
    //Rays should be a fixed distance, this should create a radius of light with shadows. (will look jittery in motion without adjustments)
    //MESH TRANSFORM MIGHT BE OUT OF SYNC? DOUBLE CHECK THIS ISSUE!
    void Update()
    {
        Vector3 center = this.transform.position;
        lightMesh.Clear();
        vertices[0] = new Vector3(0,0,transform.position.z);
        triangles[0] = 0;
        for(int i = 1;i < vertAmount; i++)
        {
            startDir.x = lightRadius * Mathf.Cos(Mathf.Deg2Rad*(angleStep * i));
            startDir.y = lightRadius * Mathf.Sin(Mathf.Deg2Rad*(angleStep * i));
            RaycastHit2D ray = Physics2D.Raycast(center, startDir, lightRadius);
            
            if(ray)
            {
                float distance = Vector2.Distance(ray.point, new Vector2(center.x, center.y));
                Vector3 localPoint = new Vector3(ray.point.x - center.x, ray.point.y - center.y, center.z);
                //vertices[i] = new Vector3(ray.point.x,ray.point.y,transform.position.z);
                Debug.DrawLine(transform.position, ray.point);
                Vector3 dirTest = new Vector3(ray.point.x - center.x, ray.point.y - center.y, center.z);
                Debug.DrawLine(transform.position, transform.position + dirTest,Color.red);
                vertices[i] = localPoint;
            }
            else
            {
                //vertices[i] = new Vector3(center.x + startDir.x,center.y+startDir.y,center.z);
                vertices[i] = new Vector3(startDir.x,startDir.y, center.z);
            }
            
//            vertices[i] = new Vector3(ray.point.x, ray.point.y, transform.position.z);
        }
        int vertTrack = 1;
        for(int j = 1; j < triangles.Length; j++)
        {
            if (j % 3 == 0)
            {
                triangles[j] = 0;
                vertTrack--;
            }
            else
            {
                if (j == triangles.Length - 2) vertTrack = vertAmount - 1;
                triangles[j] = vertTrack;
                if (vertTrack >= vertAmount - 1) vertTrack = 1;
                else vertTrack++;
                
            }
        }
        lightMesh.vertices = vertices;
        lightMesh.triangles = triangles;
        meshFilter.mesh = lightMesh;
    }
}
/*
 * BACKUP CODE
 * void Update()
    {
        Vector3 center = this.transform.position;
        lightMesh.Clear();
        vertices[0] = center;
        triangles[0] = 0;
        for(int i = 1;i < vertAmount; i++)
        {
            startDir.x = lightRadius * Mathf.Cos(angleStep * i);
            startDir.y = lightRadius * Mathf.Sin(angleStep * i);
            RaycastHit2D ray = Physics2D.Raycast(center, startDir, lightRadius);
            
            if(ray)
            {
                vertices[i] = new Vector3(ray.point.x,ray.point.y,transform.position.z);
            }
            else
            {
                vertices[i] = new Vector3(center.x + startDir.x,center.y+startDir.y,center.z);
            }
            
//            vertices[i] = new Vector3(ray.point.x, ray.point.y, transform.position.z);
        }
        int vertTrack = 1;
        for(int j = 1; j < triangles.Length; j++)
        {
            if (j % 3 == 0)
            {
                triangles[j] = 0;
                vertTrack--;
            }
            else
            {
                triangles[j] = vertTrack;
                if (vertTrack == vertAmount - 1) vertTrack = 0;
                vertTrack++;
            }
        }
        lightMesh.vertices = vertices;
        lightMesh.triangles = triangles;
        meshFilter.mesh = lightMesh;
    }
 */
 


