using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotBoss : MonoBehaviour
{

    public enum BossState
    {
        Disabled,
        Idle,
        MoveToPlayer,
        MoveFromPlayer,
        Dead
    }

    [SerializeField]
    private float _moveForwardSpeed;

    [SerializeField]
    private float _moveBackSpeed;

    [SerializeField]
    private float _attackMinDelay;

    [SerializeField]
    private float _comboChance;

    [SerializeField]
    private float _moveFromPlayerTime;

    [SerializeField]
    private float _minAttackRange;

    private static readonly int Enable = Animator.StringToHash("Enable");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int AttackType = Animator.StringToHash("AttackType");
    private static readonly int AttackTypesAmount = 2;

    private BossState _state;
    private int _attackType;

    private Animator _animator;
    private Health _health;
    private float _prevHealth;
    private Transform _playerTransform;
    private Collider2D _collider;

    private float _attackTimer;
    private float _moveFromPlayerTimer;

    private Transform Player
    {
        get
        {
            if (_playerTransform == null)
            {
                _playerTransform = FindAnyObjectByType<PlayerController>().transform;
            }

            return _playerTransform ?? transform;
        }
    }

    private Vector3 DirectionToPlayer
    {
        get
        {
            Vector3 direction = Player.transform.position - transform.position;
            if (direction == Vector3.zero)
            {
                direction.x += 0.001f;
            }
            return direction;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _prevHealth = _health.CurrentHealth;
        _attackTimer = 0.0f;
        _moveFromPlayerTimer = 0.0f;
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;
        _health.enabled = false;
    }


    private void Update()
    {
        if (_state == BossState.Disabled || _state == BossState.Dead)
        {
            return;
        }

        if (_health.IsDead)
        {
            _state = BossState.Dead;

            return;
        }
        _attackTimer = Mathf.Max(0.0f, _attackTimer - Time.deltaTime);

        if (_state != BossState.MoveFromPlayer)
        {
            CheckMovement();
        }

        CheckAttack();
        CheckDamageReceived();

        _prevHealth = _health.CurrentHealth;

        _animator.SetBool(IsMoving, _state == BossState.MoveToPlayer || _state == BossState.MoveFromPlayer);

        int direction;
        float speed;
        if (_state == BossState.MoveFromPlayer)
        {
            float xDiff = DirectionToPlayer.x;
            if (xDiff > 0.0f)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
            speed = _moveBackSpeed;

            _moveFromPlayerTimer += Time.deltaTime;
            if (_moveFromPlayerTimer > _moveFromPlayerTime)
            {
                _state = BossState.Idle;
            }
        }
        else if (_state == BossState.MoveToPlayer)
        {
            float xDiff = DirectionToPlayer.x;
            if (xDiff < 0.0f)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
            speed = _moveForwardSpeed;
        }
        else
        {
            float xDiff = DirectionToPlayer.x;
            if (xDiff < 0.0f)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
            speed = 0;
        }

        var oldPosition = transform.position;
        oldPosition.x += direction * speed * Time.deltaTime;
        transform.position = oldPosition;

        Vector3 scale = transform.localScale;
        scale.x = direction * Mathf.Abs(scale.x);
        transform.localScale = scale;

    }

    public void ActivateBoss()
    {
        _animator.SetTrigger(Enable);
        _collider.enabled = true;
        _health.enabled = true;
        _state = BossState.Idle;
    }

    public bool IsActive
    {
        get
        {
            return _state != BossState.Disabled;
        }
    }

    public bool IsDead
    {
        get
        {
            return _state == BossState.Dead;
        }
    }

    private void CheckAttack()
    {
        Vector3 direction = DirectionToPlayer;
        if (direction.magnitude < _minAttackRange && _attackTimer <= 0)
        {
            _animator.SetTrigger(Attack);
            _animator.SetInteger(AttackType, _attackType);
            _attackType = (_attackType + 1) % AttackTypesAmount;
            _attackTimer = _attackMinDelay;
            if (Random.Range(0, 1) < _comboChance)
            {
                StartCoroutine(ComboAttack());
            }

        }
    }

    private void CheckMovement()
    {
        Vector3 direction = DirectionToPlayer;

        if (direction.magnitude < _minAttackRange / 2)
        {
            _state = BossState.Idle;
        }
        else
        {
            _state = BossState.MoveToPlayer;
        }
    }

    private void CheckDamageReceived()
    {
        if (_prevHealth > _health.CurrentHealth)
        {
            _state = BossState.MoveFromPlayer;
            _moveFromPlayerTimer = 0.0f;
        }
    }

    private IEnumerator ComboAttack()
    {
        yield return new WaitForSeconds(0.2f);
        _animator.SetTrigger(Attack);
        _animator.SetInteger(AttackType, _attackType);
        _attackType = (_attackType + 1) % AttackTypesAmount;
    }

}
