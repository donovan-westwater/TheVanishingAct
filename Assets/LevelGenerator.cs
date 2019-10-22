using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    //SET IN EDITOR
    public GameObject[] traversables;
    public GameObject[] puzzles;
    public GameObject[] obstacles;
    List<Transform> puzzlePoints;
    List<Transform> obstaclePoints;
    int totalMana = 0;
    //Place here a dicitonary of skills as key and amount of of skill as value. This should be public
    public Dictionary<string, int> skillCount; // skill names Teleport, Guard, Partol, Grab,Move
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, traversables.Length - 1);
        GameObject travs = Instantiate(traversables[rand], this.transform.position, this.transform.rotation);
        foreach (Transform child in transform.Find("PuzzlePoints"))
        {
            puzzlePoints.Add(child);
           // totalMana += child.GetComponent<RoomStats>().mana;
        }
        foreach (Transform child in transform.Find("ObstaclePoints"))
        {
            obstaclePoints.Add(child);
        }
        //Spawning rooms
        foreach(Transform point in puzzlePoints)
        {

        }
        foreach (Transform point in obstaclePoints)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
