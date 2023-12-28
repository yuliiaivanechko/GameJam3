using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Damage;

    private Health _health;

    public void Start()
    {
        _health = GetComponent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_health == null)
        {
            return;
        }
        Sword sword = collision.GetComponent<Sword>();
        if (sword != null)
        {
            _health.TakeDamage(sword.Damage);
            if (_health.IsDead)
            {
                Damage = 0;
                StartCoroutine(KillEnemy());
            }
        }
    }

    public bool IsDead
    {
        get
        {
            return _health?.IsDead ?? false;
        }
    }

    private IEnumerator KillEnemy()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

}
