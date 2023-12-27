using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager instance { get; private set; }

    private GameData gameData; 

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found several data persistance managers in the scene");
        }
        instance = this;
    }

    private void Start()
    {
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        if (this.gameData == null)
        {
            Debug.Log("No data 3was found. Initializing data to defaults");
            NewGame();
        }
    }

    public void SaveGame()
    {

    }
}
