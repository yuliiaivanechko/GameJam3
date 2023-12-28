using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour, IDataPersistance
{
    [SerializeField]
    private GameObject _bossPrefab;

    [SerializeField]
    private int _maxBossesAtTheSameTime;

    [SerializeField]
    private float _bossDistance;

    [SerializeField]
    private float _defaultBossesNumber;

    private List<RobotBoss> _bosses;

    public void LoadData(GameData data, string prevScene)
    {

        int catsKilled = 0;
        foreach (var kv in data.catsKilled)
        {
            if (kv.Value)
            {
                catsKilled++;
            }
        }
        _bosses = new List<RobotBoss>();

        for (int i = 0; i < _defaultBossesNumber + 2 * catsKilled; i++)
        {
            Vector3 pos = transform.position;
            pos.x += i * _bossDistance;

            _bosses.Add(Instantiate(_bossPrefab, transform).GetComponent<RobotBoss>());
            _bosses[_bosses.Count - 1].transform.position = pos;
        }
    }

    public void SaveData(ref GameData data)
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_bosses.Count == 0)
        {
            return;
        }

        _bosses.RemoveAll((boss) => boss.IsDead);
        int active = _bosses.FindAll((boss) => boss.IsActive).Count;
        if (active < _maxBossesAtTheSameTime)
        {
            for (int i = 0; i < _bosses.Count; i++)
            {
                if (!_bosses[i].IsActive)
                {
                    _bosses[i].ActivateBoss();
                    active++;
                    if (active >= _maxBossesAtTheSameTime)
                    {
                        break;
                    }
                    
                }
            }
        }

    }
}
