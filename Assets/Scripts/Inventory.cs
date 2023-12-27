using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IDataPersistance
{
    private bool dash = false;
    private bool wall = false;
    private bool doubleJump = false;

    private SpriteRenderer visual;


    private void Awake()
    {
        visual = GetComponent<SpriteRenderer>();
    }

    public void LoadData(GameData data, string prevScene)
    {
        this.wall = data.wall;
        this.dash = data.dash;
        this.doubleJump = data.doubleJump;
    }

    public void SaveData(ref GameData data)
    {
        data.wall = this.wall;
        data.dash = this.dash;
        data.doubleJump = this.doubleJump;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        visual.gameObject.SetActive(false);
        if (col.gameObject.CompareTag("Wall"))
        {
            wall = true;
        }
        else if (col.gameObject.CompareTag("Dash"))
        {
            dash = true;
        }
        else if (col.gameObject.CompareTag("DoubleJump"))
        {
            doubleJump = true;
        }
    }
}
