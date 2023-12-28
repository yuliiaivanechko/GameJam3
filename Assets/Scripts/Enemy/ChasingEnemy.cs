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

    private PlayerHealth ToChase
    {
        get
        {
            if (_toChase == null)
            {
                _toChase = FindAnyObjectByType<PlayerHealth>();
            }
            return _toChase;
        }
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (ToChase?.IsDead ?? true)
        {
            return;
        }

        Vector3 currentPosition = transform.position;
        Vector3 direction = (ToChase.transform.position - currentPosition).normalized;
        currentPosition += _speed * Time.deltaTime * direction;
        transform.position = currentPosition;

        if ((ToChase.transform.position - transform.position).magnitude < _attackAnimationRange)
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
