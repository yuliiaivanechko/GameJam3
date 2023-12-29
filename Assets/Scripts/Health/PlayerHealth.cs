using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : Health
{
    private static bool _isInited = false;
    private static float _health = 0;
    private float HeartCount;

    public Image[] Hearts;

    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    protected override float GetCurrentHealth()
    {
        if (!_isInited)
        {
            ResetHealth();
            _isInited = true;
        }
        return _health;
    }
    protected override void SetCurrentHealth(float value)
    {
        _health = value;
    }
    private void UpdateUIHearts() 
    {
        bool isHalfHeartActive = false;
        if((_health / 10)% 2  == 1)
        {
            isHalfHeartActive = true;
        }
        float heartCount = _health / 20; // full hearts 
        for(int i = 0; i < Hearts.Length; i++)
        {
           if (i < heartCount)
           {
               Hearts[i].sprite = fullHeart;
           }
           else if(i == heartCount && isHalfHeartActive)
           {
               Hearts[i].sprite = halfHeart;
           }
           else
           {
               Hearts[i].sprite = emptyHeart;
           }
        }
    }
}
