using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{

    private float _currentHealth;
    
    protected void Start()
    {
        ResetHealth();
    }

    protected override float GetCurrentHealth()
    {
        return _currentHealth;
    }

    protected override void SetCurrentHealth(float health)
    {
        _currentHealth = health;
    }

}
