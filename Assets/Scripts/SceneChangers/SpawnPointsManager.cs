using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPointsManager : MonoBehaviour, IDataPersistance
{

    [SerializeField]
    private List<Transform> _spawnPoints;

    [SerializeField]
    private List<string> _previousScenes;

    [SerializeField]
    private Transform _defaultSpawnPoint;

    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private float _YspawnDelta;

    private void Start()
    {
    }

    public void LoadData(GameData data, string prevScene)
    {
        int _sceneIndex = _previousScenes.FindIndex((name) => name == prevScene);
        Vector3 pos;
        Quaternion rotation;
        if (_sceneIndex != -1)
        {
            pos = _spawnPoints[_sceneIndex].position;
            rotation = _spawnPoints[_sceneIndex].rotation;

        }
        else
        {
            pos = _defaultSpawnPoint?.position ?? new Vector3(0.0f, 0.0f);
            rotation = _defaultSpawnPoint.rotation;
        }
        pos.y += _YspawnDelta;

        GameObject obj = Instantiate(_playerPrefab, pos, rotation);
        var virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        virtualCamera.Follow = obj.transform;
    }
    public void SaveData(ref GameData data)
    {

    }
}
