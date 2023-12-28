using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour, IDataPersistance
{
    private bool doubleJump = false;

    private SpriteRenderer visual;


    private void Awake()
    {
        visual = GetComponent<SpriteRenderer>();
    }

    public void LoadData(GameData data, string prevScene)
    {
        if (data.doubleJump)
        {
            visual.gameObject.SetActive(false);
        }
        this.doubleJump = data.doubleJump;
    }

    public void SaveData(ref GameData data)
    {
        data.doubleJump = this.doubleJump;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            visual.gameObject.SetActive(false);
            this.doubleJump = true;
        }
        GameData data = DataPersistanceManager.instance.GetData();
        data.doubleJump = this.doubleJump;
    }
}
