using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cock : MonoBehaviour, IDataPersistance
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private SpriteRenderer visual;
    private bool collected = false;



    private void Awake()
    {
        visual = this.GetComponentInChildren<SpriteRenderer>();
    }

    public void LoadData(GameData data)
    {
        data.cocksCollected.TryGetValue(id, out collected);
        if (collected)
        {
            visual.gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.cocksCollected.ContainsKey(id))
        {
            data.cocksCollected.Remove(id);
        }
        data.cocksCollected.Add(id, collected);
    }

    private void OnTriggerEnter2D()
    {
        if (!collected)
        {
            CollectCock();
        }
    }

    void Start()
    {
        
    }

    void CollectCock()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
