using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] 
    private float attackCooldown;

    [SerializeField] 
    private float range;

    [SerializeField]
    private GameObject bullet;

    [Header("Player Layer")]
    [SerializeField]
    private LayerMask playerLayer;

    private BoxCollider2D _boxCollider;
    private float cooldownTimer = Mathf.Infinity;
    private Animator _animator;
    private EnemyPatrol _enemyPatrol;
    private Vector3 _prevPosition;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyPatrol = GetComponentInParent<EnemyPatrol>();
        _prevPosition = transform.position;
        _boxCollider = GetComponent<BoxCollider2D>();
        _prevPosition = transform.position;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        _animator.SetBool("isWalking", _prevPosition != transform.position);
        _prevPosition = transform.position;

        //Attack only when player in sight?
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                _animator.SetTrigger("attack");
            }
        }

        _enemyPatrol.enabled = !PlayerInSight();
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(_boxCollider.bounds.center,
            new Vector3(_boxCollider.bounds.size.x * range, _boxCollider.bounds.size.y, _boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}


