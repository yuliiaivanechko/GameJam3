using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int xp;
    public int cockToken = 0;
    public bool dash = false;
    public bool wall = false;
    public int currency = 20;

    public GameData()
    {
        this.xp = 100;
        this.cockToken = 0;
        this.currency = 100;
        this.dash = false;
        this.wall = false;
    }

}
