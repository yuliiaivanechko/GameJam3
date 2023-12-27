using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    void LoadData(GameData data, string prevScene);

    void SaveData(ref GameData data);
}
