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
    //skillCount is the dictioary for how much of each skill should be present on the map at the current time
    public Dictionary<string, int> skillCount; // skill names Teleport, Guard, Partol, Grab,Move (Puzzles should teach different skills to the obstacles!)
    Dictionary<string, List<GameObject>> skillMap = new Dictionary<string, List<GameObject>>();
    // Start is called before the first frame update
    void Start()
    {
        //Randomly selecting mansion layout and then gathering its puzzle and obstacle spawn points
        //Remember the points store both location AND rotation for the rooms!
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
        //Go through the puzzles and obstcles to gather and organize the puzzles and obstacles into skill catagories
        foreach(GameObject g in puzzles)
        {
            string checkSkill = g.GetComponent<RoomStats>().skill;
            if (skillMap.ContainsKey(checkSkill))
            {
                skillMap[checkSkill].Add(g);
            }
            else
            {
                List<GameObject> insert = new List<GameObject>();
                insert.Add(g);
                skillMap.Add(checkSkill,insert);
            }
        }
        foreach (GameObject g in obstacles)
        {
            string checkSkill = g.GetComponent<RoomStats>().skill;
            if (skillMap.ContainsKey(checkSkill))
            {
                skillMap[checkSkill].Add(g);
            }
            else
            {
                List<GameObject> insert = new List<GameObject>();
                insert.Add(g);
                skillMap.Add(checkSkill, insert);
            }
        }
        //Spawning rooms: Go through skillCount and pick a random puzzle point and a random puzzle for that skill out 
        //of the map, Repeat this process for obstacles
        foreach(string k in skillCount.Keys)
        {
            //Pick a random puzzle/obstacle out of map,random Point, decriment skillCount value for the key 
            //and pop off the point from the corresponding list
            while(skillCount[k] > 0)
            {
                rand = Random.Range(0, skillMap[k].Count-1);
                int pointRand = 0;
                Transform spawnPoint;
                GameObject spawn = skillMap[k][rand];
                if (spawn.CompareTag("Puzzle"))
                {
                    pointRand = Random.Range(0, puzzlePoints.Count - 1);
                    spawnPoint = puzzlePoints[pointRand];
                    puzzlePoints.RemoveAt(pointRand);
                }
                else
                {
                    pointRand = Random.Range(0, obstaclePoints.Count - 1);
                    spawnPoint = obstaclePoints[pointRand];
                    obstaclePoints.RemoveAt(pointRand);
                }
                skillCount[k] -= 1;

            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
