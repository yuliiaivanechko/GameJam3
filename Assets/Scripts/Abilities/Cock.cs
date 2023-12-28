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

    [SerializeField]
    private AudioSource coco;

    private void Awake()
    {
        visual = this.GetComponentInChildren<SpriteRenderer>();
    }

    public void LoadData(GameData data, string prevScene)
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
        Debug.Log("Shut up chicken");
        if (!collected)
        {
            coco.Play();
            StartCoroutine(WaitForCollecting());
            collected = true;
        }
    }


    IEnumerator WaitForCollecting()
    {
        // Wait for the duration of the "Death" animation
        yield return new WaitForSeconds(3);

        // Destroy the cat GameObject after the animation finishes
        visual.gameObject.SetActive(false);
    }

}
