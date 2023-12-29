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

    private Enemy[] _enemies;

    // Start is called before the first frame update
    private void Start()
    {
        _enemies = FindObjectsOfType<Enemy>();
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
        Enemy entity = gameObject.GetComponent<Enemy>();
        if (entity != null)
        {
            FearData.TrackKilledEnemy(sceneName, entity.ID);
        }
    }

    public void HitPlayer()
    {
        ChangeFearLevel(_fearPerHit);
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
        if (_enemies.Length > killedEnemies.Count)
        {
            FearData.ChangeFearLevel(_fearPerScene);
        }

        Debug.Log(FearData.fearLevel);
    }

    private void SetInitialIDS()
    {
        int id = 0;
        foreach (Enemy entity in _enemies)
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
        foreach (Enemy entity in _enemies)
        {
            entity.gameObject.SetActive(!killedEnemies.Contains(entity.ID) && fearLevel >= entity.MinFearLevel);
        }
    }
}
