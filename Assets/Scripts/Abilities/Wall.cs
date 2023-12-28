using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IDataPersistance
{
    private bool wall = false;

    private SpriteRenderer visual;


    private void Awake()
    {
        visual = GetComponent<SpriteRenderer>();
    }

    public void LoadData(GameData data, string prevScene)
    {
        this.wall = data.wall;

    }

    public void SaveData(ref GameData data)
    {
        data.wall = this.wall;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            visual.gameObject.SetActive(false);
            this.wall = true;
        }
    }
}
