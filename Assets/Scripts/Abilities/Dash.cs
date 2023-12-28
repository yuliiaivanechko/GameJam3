using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour, IDataPersistance
{
    private bool dash = false;

    private SpriteRenderer visual;


    private void Awake()
    {
        visual = GetComponent<SpriteRenderer>();
    }

    public void LoadData(GameData data, string prevScene)
    {
        this.dash = data.dash;
    }

    public void SaveData(ref GameData data)
    {
        data.dash = this.dash;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            visual.gameObject.SetActive(false);
            this.dash = true;
        }
    }
}
