using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
    GameObject player;
    GameObject perspectiveMangager;
    private int mana;
    ///bool[] activeShifts;
    GameObject[] manaOrbs = new GameObject[3];
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").gameObject;
        for(int i = 0; i < 3; i++)
        {
            manaOrbs[i] = this.transform.GetChild(i).gameObject;
        }
        perspectiveMangager = GameObject.Find("PrespectiveManager").gameObject;
       // activeShifts = perspectiveMangager.GetComponent<PerspectiveManager>().shiftActive();
        mana = player.GetComponent<Player_Controls>().getMana();
    }

    // Update is called once per frame
    void Update()
    {
        mana = player.GetComponent<Player_Controls>().getMana();
        // activeShifts = perspectiveMangager.GetComponent<PerspectiveManager>().shiftActive();
        switch (mana)
        {
            case 3:
                for (int x = 0; x < 3; x++) manaOrbs[x].GetComponent<Image>().enabled = true;
                break;
            case 2:
                for (int x = 0; x < 2; x++) manaOrbs[x].GetComponent<Image>().enabled = true;
                manaOrbs[2].GetComponent<Image>().enabled = false;
                break;
            case 1:
                manaOrbs[2].GetComponent<Image>().enabled = false;
                manaOrbs[1].GetComponent<Image>().enabled = false;
                manaOrbs[0].GetComponent<Image>().enabled = true;
                break;
            case 0:
                for (int x = 0; x < 3; x++) manaOrbs[x].GetComponent<Image>().enabled = false;
                break;

        }
    }
}
