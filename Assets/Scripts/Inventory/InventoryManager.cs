using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    public GameData gameData;
    public GameObject Dash;
    public GameObject DoubleJump;
    public GameObject Cock;
    public GameObject WallClimb;
   
    void Start()
    {
        gameData = DataPersistanceManager.instance.GetData();
        if (gameData.dash)
            Dash.SetActive(true);
        else { Dash.SetActive(false); }
        if (gameData.doubleJump)
            DoubleJump.SetActive(true);
        else { DoubleJump.SetActive(false); }
        if (gameData.wall)
            WallClimb.SetActive(true);
        else { WallClimb.SetActive(false); }

    }

    void Update()
    {
        
    }

    int getCockNumber()
    {
        int count = 0;
        if (gameData.cocksCollected != null)
        {

            for (int i = 0; i < gameData.cocksCollected.values.Count; i++)
            {
                if (gameData.cocksCollected.values[i])
                {
                    count++;
                }
            }
        }
        return count;
    }
}
