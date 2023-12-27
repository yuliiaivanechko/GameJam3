using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IDataPersistance
{
    public bool dash = false;
    public bool wall = false;
    public int currency = 20;


    public void LoadData(GameData data, string prevScene)
    {
        this.wall = data.wall;
        this.dash = data.dash;
        this.currency = data.currency;
    }

    public void SaveData(ref GameData data)
    {
        data.wall = this.wall;
        data.dash = this.dash;
        data.currency = this.currency;
    }
}
