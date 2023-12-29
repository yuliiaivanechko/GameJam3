using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    public static DataPersistanceManager instance { get; private set; }

    private List<IDataPersistance> dataPersistanceObjects;

    public GameData gameData;

    private FileDataHandler dataHandler;
    private string prevScene;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found several data persistance managers in the scene");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame(prevScene);
    }

    public GameData GetData()
    {
        return gameData;
    }

    public void OnSceneUnloaded(Scene scene)
    {
        prevScene = scene.name;
    }

    public void NewGame()
    {
        Debug.Log("new data");
        dataHandler.Delete();
        this.gameData = new GameData();

    }

    public void LoadGame(string prevScene)
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No data 3was found.");
            this.gameData = new GameData();
        }

        foreach (IDataPersistance dataPeristanceObj in dataPersistanceObjects)
        {
            dataPeristanceObj.LoadData(gameData, prevScene);
        }
    }

    public void SaveGame()
    {
        if (this.gameData == null)
        {
            Debug.LogWarning("No data found");
            return;
        }

        foreach (IDataPersistance dataPeristanceObj in dataPersistanceObjects)
        {
            dataPeristanceObj.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

}
