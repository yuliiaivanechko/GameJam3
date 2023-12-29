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
        Dash.SetActive(false);
        DoubleJump.SetActive(false);
        WallClimb.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    int getCockNumber()
    {
        int count = 0;
      /*  if (gameData.cocksCollected != null)
        {

            for (int index = 0; index < gameData.cocksCollected.Count; index++)
            {
                bool itemValue = gameData.cocksCollected[index].Value;
                var item = gameData.cocksCollected[index];
                bool itemValue = item.Value;
                if (itemValue == true)
                    count++;
            }
        }*/
        return count;
    }
}
