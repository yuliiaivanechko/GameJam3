using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrize : MonoBehaviour
{
    [SerializeField]
    private GameObject _prize;

    [SerializeField]
    private Transform _prizetransform;

    private void OnDisable()
    {
        Instantiate(_prize, _prizetransform);
    }
}
