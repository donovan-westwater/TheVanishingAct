using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStorage : MonoBehaviour
{
    public static GameStorage Instance;
    public int levelSeed = 0;
    public Vector3 playerpos;
    public bool isSaveLoad = false;
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
