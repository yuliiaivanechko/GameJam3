using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FearController : MonoBehaviour
{
    public class FearInfo
    {
        public int fearLevel;
        public Dictionary<string, HashSet<int>> killedEnemies;

        public void TrackKilledEnemy(string sceneName, int id)
        {
            killedEnemies.TryAdd(sceneName, new HashSet<int>());
            killedEnemies[sceneName].Add(id);
        }

        public void ResetKilledEnemies()
        {
            killedEnemies.Clear();
        }

        public void ChangeFearLevel(int delta)
        {
            fearLevel = Mathf.Min(Mathf.Max(fearLevel + delta, 0), 100);
        }

        public void SetFearLevel(int level)
        {
            fearLevel = Mathf.Min(Mathf.Max(level, 0), 100);
        }
    }

    [SerializeField]
    private int _fearPerHit;

    [SerializeField]
    private int _fearPerScene;

    private static FearInfo _fearData = null;

    public FearInfo FearData
    {
        get
        {
            if (_fearData == null)
            {
                _fearData = new FearInfo()
                {
                    fearLevel = 0,
                    killedEnemies = new Dictionary<string, HashSet<int>>()
                };
            }
            return _fearData;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        SetInitialIDS();
        UpdateFearLevelAfterEntering();
        UpdateEntitiesBasedOnFearLevel();
    }

    public void SetFearLevel(int value)
    {
        FearData.SetFearLevel(value);
        UpdateEntitiesBasedOnFearLevel();
    }

    public void ChangeFearLevel(int delta)
    {
        FearData.ChangeFearLevel(delta);
        UpdateEntitiesBasedOnFearLevel();
    }

    public void TrackKilledEnemy(GameObject gameObject)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        FearEntity entity = gameObject.GetComponent<FearEntity>();
        if (entity != null)
        {
            FearData.TrackKilledEnemy(sceneName, entity.ID);
        }
    }

    public void HitPlayer(GameObject gameObject)
    {
        FearEntity entity = gameObject.GetComponent<FearEntity>();
        if (entity != null)
        {
            ChangeFearLevel(_fearPerHit);
        }
    }

    public void ResetKilledEnemies()
    {
        FearData.ResetKilledEnemies();
    }

    private void UpdateFearLevelAfterEntering()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        HashSet<int> killedEnemies;
        if (!FearData.killedEnemies.TryGetValue(sceneName, out killedEnemies))
        {
            killedEnemies = new HashSet<int>();
        }
        if (gameObject.GetComponentsInChildren<FearEntity>().Length > killedEnemies.Count)
        {
            FearData.ChangeFearLevel(_fearPerScene);
        }

        Debug.Log(FearData.fearLevel);
    }

    private void SetInitialIDS()
    {
        int id = 0;
        foreach (FearEntity entity in gameObject.GetComponentsInChildren<FearEntity>())
        {
            entity.ID = id;
            id++;
        }
    }

    private void UpdateEntitiesBasedOnFearLevel()
    {
        int fearLevel = FearData.fearLevel;
        string sceneName = SceneManager.GetActiveScene().name;
        HashSet<int> killedEnemies;
        if (!FearData.killedEnemies.TryGetValue(sceneName, out killedEnemies))
        {
            killedEnemies = new HashSet<int>();
        }
        foreach (FearEntity entity in gameObject.GetComponentsInChildren<FearEntity>())
        {
            entity.gameObject.SetActive(!killedEnemies.Contains(entity.ID) && fearLevel >= entity.MinFearLevel);
        }
    }
}
