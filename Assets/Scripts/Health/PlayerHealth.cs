using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    private static bool _isInited = false;
    private static float _health = 0;

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
}
