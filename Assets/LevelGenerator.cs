using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    //SET IN EDITOR
    public GameObject[] traversables;
    public GameObject[] puzzles;
    public GameObject[] obstacles;
    public string[] skills; //SHOULD BE THE SAME SIZE OF COUNT
    public int[] count; //SHOULD BE THE SAME SIZE OF SKILLS
    public int levelSeed;
    List<Transform> puzzlePoints = new List<Transform>();
    List<Transform> obstaclePoints = new List<Transform>();
    int totalMana = 0;
    //skillCount is the dictioary for how much of each skill should be present on the map at the current time
    public Dictionary<string, int> skillCount = new Dictionary<string, int>(); // skill names Teleport, Guard, Partol, Grab,Move (Puzzles should teach different skills to the obstacles!)
    Dictionary<string, List<GameObject>> skillMap = new Dictionary<string, List<GameObject>>();
    // Start is called before the first frame update
    void Start()
    {
        //Load Random state from save here/ Create new Random State ! (check if data is empty)
        if (GameStorage.Instance.isSaveLoad)
        {
            GameObject.Find("Player").transform.position = GameStorage.Instance.playerpos;
            //Random.state = GameStorage.Instance.levelState;
            Random.InitState(GameStorage.Instance.levelSeed);
            GameStorage.Instance.isSaveLoad = false;
        } else
        {
            levelSeed = System.DateTime.Now.Millisecond;
            Random.InitState(levelSeed);
        }
        for(int i = 0;i < count.Length;i++)
        {
            skillCount.Add(skills[i], count[i]);
        }
        
        //Randomly selecting mansion layout and then gathering its puzzle and obstacle spawn points
        //Remember the points store both location AND rotation for the rooms!
        int rand = Random.Range(0, traversables.Length - 1);
        GameObject travs = Instantiate(traversables[rand], this.transform.position, this.transform.rotation);
        foreach (Transform child in travs.transform.Find("PuzzlePoints").transform)
        {
            puzzlePoints.Add(child);
           // totalMana += child.GetComponent<RoomStats>().mana;
        }
        foreach (Transform child in travs.transform.Find("ObstaclePoints").transform)
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
        Dictionary<string, int> decrimentTracker = new Dictionary<string, int>();
        foreach (string k in skillCount.Keys)
        {
            //Pick a random puzzle/obstacle out of map,random Point, decriment skillCount value for the key 
            //and pop off the point from the corresponding list

            decrimentTracker.Add(k, 0);
            while(skillCount[k] - decrimentTracker[k] > 0)
            {
                rand = Random.Range(0, skillMap[k].Count);
                int pointRand = 0;
                Transform spawnPoint;
                GameObject spawn = skillMap[k][rand];
                int check = totalMana;
                check += spawn.GetComponent<RoomStats>().mana;
                if (check < 0) continue;
                totalMana += spawn.GetComponent<RoomStats>().mana;
                if (spawn.CompareTag("Puzzle"))
                {
                    pointRand = Random.Range(0, puzzlePoints.Count);
                    spawnPoint = puzzlePoints[pointRand];
                    GameObject clone = Instantiate(spawn, spawnPoint.transform.position,spawnPoint.transform.rotation,spawnPoint.transform) as GameObject; //Rotation Wonky
                    //clone.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));

                   // clone.transform.root.localEulerAngles = new Vector3(0, 0, spawnPoint.eulerAngles.z); //Consider going through the gameobjects children and having them rotate around the parent's center the same amount
                    //clone.transform.RotateAround(spawnPoint.position, new Vector3(0, 0, 1), spawn.transform.rotation.eulerAngles.z);
                    //clone.transform.localRotation = new Quaternion(clone.transform.rotation.x, clone.transform.rotation.y,spawnPoint.transform.rotation.z, clone.transform.rotation.w);
                    //clone.transform.root.transform.Rotate(0,0,spawnPoint.transform.rotation.eulerAngles.z,Space.Self);
                    clone.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, travs.transform.position.z);
                    puzzlePoints.RemoveAt(pointRand);
                   
                }
                else
                {
                    pointRand = Random.Range(0, obstaclePoints.Count);
                    spawnPoint = obstaclePoints[pointRand];
                    GameObject clone = Instantiate(spawn, spawnPoint.transform.position,spawnPoint.transform.rotation,spawnPoint.transform) as GameObject; //Rotation Wonky 
                    
                    //clone.transform.root.rotation = spawnPoint.rotation;
                    // clone.transform.localRotation = new Quaternion(clone.transform.rotation.x, clone.transform.rotation.y, spawnPoint.transform.rotation.z, clone.transform.rotation.w);
                    //clone.transform.root.transform.Rotate(0, 0, spawnPoint.transform.rotation.eulerAngles.z, Space.Self);
                    clone.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, travs.transform.position.z);
                    obstaclePoints.RemoveAt(pointRand);
                }
                decrimentTracker[k] += 1;
            }
        }
        AstarPath.active.Scan();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            NewRun();
        }else if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("I HAVE SAVED");
            SaveGame();
        }else if (Input.GetKeyDown(KeyCode.N))
        {
            LoadGameAndRestart();
        }
    }
    private Save CreateSaveFile()
    {
        Save save = new Save();
        save.levelSeed = levelSeed;
        Vector3 vec = GameObject.Find("Player").transform.position;
        save.playerposX = vec.x;
        save.playerposY = vec.y;
        save.playerposZ = vec.z;
        return save;
    }
    public void SaveGame()
    {
        Save save = CreateSaveFile();
        //Writes the file to disk
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

    }
    //Load data from save into GameStorage (Global Singleton) and restart
    public void LoadGameAndRestart()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            GameStorage.Instance.levelSeed = save.levelSeed;
            GameStorage.Instance.playerpos = new Vector3(save.playerposX,save.playerposY,save.playerposZ);
            GameStorage.Instance.isSaveLoad = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    //Load data from save into level (Call at start/ attach to a menu button. This also only a rough framework)[Kinda useless, might delete later]
    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            GameObject.Find("Player").transform.position = new Vector3(save.playerposX, save.playerposY, save.playerposZ);
            levelSeed = GameStorage.Instance.levelSeed;
            
        }
    }
    //restart game with new seed [Exists for testing purposes mainly]
    public void NewRun()
    {
        GameStorage.Instance.isSaveLoad = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
}
