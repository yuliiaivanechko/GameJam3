using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour, IDataPersistance, IInteractable
{
    public void Interact(GameObject player)
    {
        DataPersistanceManager.instance.SaveGame();
    }

    public void LoadData(GameData data, string prevScene)
    {
    }

    public void SaveData(ref GameData data)
    {
        data.sceneName = SceneManager.GetActiveScene().name;
    }
}
