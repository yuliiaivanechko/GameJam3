using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int xp;
    public SerializableDictionary<string, bool> cocksCollected;
    public SerializableDictionary<string, bool> catsKilled;
    public bool dash = false;
    public bool wall = false;
    public bool doubleJump = false;
    public string sceneName;

    public GameData()
    {
        this.xp = 100;
        cocksCollected = new SerializableDictionary<string, bool>();
        catsKilled = new SerializableDictionary<string, bool>();
        this.doubleJump = false;
        this.dash = false;
        this.wall = false;
        sceneName = "Level_1";
    }

}
