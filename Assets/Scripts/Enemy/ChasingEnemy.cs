using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _attackAnimationRange;

    private PlayerHealth _toChase;
    private Animator _animator;

    void Start()
    {
        _toChase = FindAnyObjectByType<PlayerHealth>();
        _animator = GetComponent<Animator>();
        _animator.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_toChase.IsDead)
        {
            return;
        }

        Vector3 currentPosition = transform.position;
        Vector3 direction = (_toChase.transform.position - currentPosition).normalized;
        currentPosition += _speed * Time.deltaTime * direction;
        transform.position = currentPosition;

        if ((_toChase.transform.position - transform.position).magnitude < _attackAnimationRange)
        {
            _animator.SetTrigger("Attack");
        }

        Vector3 currentScale = transform.localScale;
        if (direction.x > 0.1)
        {
            currentScale.x = -Mathf.Abs(currentScale.x);
        }
        else if (direction.x < -0.1)
        {
            currentScale.x = Mathf.Abs(currentScale.x);
        }
        transform.localScale = currentScale;
    }
}
