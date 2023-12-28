using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;

public class WallCock : MonoBehaviour
{
    private TilemapRenderer visual;
    public CinemachineConfiner2D virtualCamera;

    // Reference to the new CinemachineConfiner2D
    public PolygonCollider2D newConfiner;
    // Start is called before the first frame update
    void Start()
    {
        if (virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineConfiner2D>();
        }
        Debug.Log(virtualCamera);
        visual = this.GetComponentInChildren<TilemapRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider has the specified name
        if (other.gameObject.name == "SwordCollider")
        {
            visual.gameObject.SetActive(false);
            if (virtualCamera != null && newConfiner != null)
            {
                virtualCamera.m_BoundingShape2D = newConfiner;
            }
            else
            {
                Debug.LogError("Cinemachine Virtual Camera or CinemachineConfiner2D not assigned.");
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
