using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Damage;

    private Health _health;
    private FearController _fearController;

    public void Start()
    {
        _health = GetComponent<Health>();
        _fearController = GetComponentInParent<FearController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_health == null || _health.IsDead)
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
        var renderers = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 10; i >= 0; i--)
        {
            foreach (var renderer in renderers)
            {
                Color color = renderer.color;
                color.a = i / 10.0f;
                renderer.color = color;
            }
            yield return new WaitForSeconds(0.1f);
        }
        _fearController?.TrackKilledEnemy(gameObject);
        Destroy(gameObject);
    }

}
