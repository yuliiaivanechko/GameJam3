using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public enum PlayerState
    {
        Idle, 
        Move, 
        Dash,
        WallJump,
        Locked
    }

    public enum PlayerAbilities
    {
        Dash,
        WallJump,
        DoubleJump
    }

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _jumpSpeed;

    [SerializeField]
    private float _dashSpeedMultiplier;

    [SerializeField]
    private float _dashDuration;

    [SerializeField]
    private float _wallJumpDuration;

    [SerializeField]
    private float _comboMaxDelay;

    [SerializeField]
    private Collider2D _bottomCollider;

    [SerializeField]
    private Collider2D _sidesCollider;

    [SerializeField]
    private LayerMask _wallLayer;

    [SerializeField]
    private AudioSource _hit;

    [SerializeField]
    private AudioSource _damage;

    [SerializeField]
    private AudioSource _death;

    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Dash = Animator.StringToHash("Dash");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int AttackType = Animator.StringToHash("AttackType");
    private static readonly int AttackTypesAmount = 3;

    private PlayerState _state;
    private float _horizontalInput;
    private int _jumpsLeft;
    private int _dashesLeft;
    private bool _isOnGround;
    private float _dashTime;
    private float _wallJumpTime;
    private float _comboTime;
    private int _attackType;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Health _health;
    private FearController _fearController;

    private HashSet<IInteractable> _activeInteracts;
    private HashSet<Enemy> _activeEnemies;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _horizontalInput = 0.0f;
        _jumpsLeft = 1;
        _dashesLeft = 1;
        _isOnGround = _bottomCollider.IsTouchingLayers(_wallLayer);
        _animator = GetComponent<Animator>();
        _dashTime = 0.0f;
        _wallJumpTime = 0.0f;
        _state = PlayerState.Idle;
        _attackType = 0;
        _activeInteracts = new HashSet<IInteractable>();
        _activeEnemies = new HashSet<Enemy>();
        _health = GetComponent<Health>();
        _fearController = GetComponent<FearController>();

    }

    private void FixedUpdate()
    {
        if (_state == PlayerState.Locked)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }

        TrackEnemyDamage();

        if (_health.IsDead)
        {
            _death.Play();
            _state = PlayerState.Locked;
            StartCoroutine(KillPlayer());
            return;
        }

        float speedMultiplier = 1.0f;
        float velocityY = _rigidbody.velocity.y;
        if (_state == PlayerState.Dash)
        {
            velocityY = 0.0f;
            speedMultiplier = _dashSpeedMultiplier;
        }
        else if (_state == PlayerState.WallJump)
        {
            speedMultiplier = -1.0f;
        }

        _rigidbody.velocity = new Vector2(speedMultiplier * _moveSpeed * _horizontalInput, velocityY);
        _animator.SetBool(IsMoving, _state == PlayerState.Move);

        Vector3 localScale = transform.localScale;
        if (_rigidbody.velocity.x > 0.0f)
        {
            localScale.x = Mathf.Abs(localScale.x);
        }
        else if (_rigidbody.velocity.x < 0.0f)
        {
            localScale.x = -Mathf.Abs(localScale.x);
        }
        transform.localScale = localScale;

        UpdateMovingState();
        UpdateJumpState();
        UpdateDashState(Time.fixedDeltaTime);
        UpdateWallJumpState(Time.fixedDeltaTime);
        UpdateComboState(Time.fixedDeltaTime);
    }

    private IEnumerator KillPlayer()
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

        _health.ResetHealth();
        _fearController.ResetKilledEnemies();
        Debug.Log("killed");
        SceneManager.LoadScene(DataPersistanceManager.instance.gameData?.sceneName ?? "MainMenu");
    }

    private bool IsAbilityUnlocked(PlayerAbilities ability)
    {
        GameData data = DataPersistanceManager.instance.GetData();
        Debug.Log(ability);
        Debug.Log(data.dash);
        if (ability == PlayerAbilities.Dash)
        {
            return data.dash;
        }
        if (ability == PlayerAbilities.WallJump)
        {
            return data.wall;
        }
        if (ability == PlayerAbilities.DoubleJump)
        {
            return data.doubleJump;
        }
        return false;
    }

    private void TrackEnemyDamage()
    {
        foreach (Enemy enemy in _activeEnemies)
        {
            float healthBefore = _health.CurrentHealth;
            _health.TakeDamage(enemy.Damage);
            if (_health.CurrentHealth != healthBefore)
            {
                _fearController.HitPlayer();
                _damage.Play();
            }
        }

        _activeEnemies.RemoveWhere(enemy => !enemy.isActiveAndEnabled || enemy.IsDead);
    }
    private void UpdateMovingState()
    {
        if (_state == PlayerState.Dash || _state == PlayerState.WallJump)
        {
            return;
        }

        if (_rigidbody.velocity.x != 0)
        {
            _state = PlayerState.Move;
        }
        else
        {
            _state = PlayerState.Idle;
        }
    }

    private void UpdateJumpState()
    {
        _isOnGround = _bottomCollider.IsTouchingLayers(_wallLayer);
        if (_isOnGround)
        {
            _jumpsLeft = 1;
            _dashesLeft = 1;
        }
    }

    private void UpdateDashState(float timeDelta)
    {
        if (_state != PlayerState.Dash)
        {
            return;
        }
        _dashTime += timeDelta;
        if (_dashTime > _dashDuration)
        {
            _dashTime = 0.0f;
            _state = PlayerState.Idle;
        }
    }

    private void UpdateWallJumpState(float timeDelta)
    {
        if (_state != PlayerState.WallJump)
        {
            return;
        }

        _wallJumpTime += timeDelta;
        if (_wallJumpTime > _wallJumpDuration)
        {
            _wallJumpTime = 0.0f;
            _state = PlayerState.Idle;
        }
    }

    private void UpdateComboState(float timeDelta)
    {
        if (_attackType == 0)
        {
            _comboTime = 0;
            return;
        }

        _comboTime += timeDelta;
        if (_comboTime > _comboMaxDelay)
        {
            _comboTime = 0;
            _attackType = 0;
            _animator.SetInteger(AttackType, _attackType);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            _activeInteracts.Add(interactable);
            interactable.ChangeState(true);
            return;
        }

        SceneChanger sceneChanger = collision.GetComponent<SceneChanger>();
        if (sceneChanger != null && _state != PlayerState.Locked)
        {
            sceneChanger.ChangeScene();
            _state = PlayerState.Locked;
            return;
        }

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && !enemy.IsDead)
        {
            _activeEnemies.Add(enemy);
            float healthBefore = _health.CurrentHealth;
            _health.TakeDamage(enemy.Damage);
            if (_health.CurrentHealth != healthBefore)
            {
                _fearController.HitPlayer();
                _damage.Play();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            _activeInteracts.Remove(interactable);
            interactable.ChangeState(false);
        }

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            _activeEnemies.Remove(enemy);
        }
    }

    private void OnMove(InputValue value)
    {
        if (_state == PlayerState.Dash || _state == PlayerState.WallJump)
        {
            return;
        }
        _horizontalInput = value.Get<float>();
    }

    private void OnJump()
    {
        if (_state == PlayerState.Dash || _state == PlayerState.Locked)
        {
            return;
        }
        if (!_isOnGround)
        {
            bool isTouchingWall = _sidesCollider.IsTouchingLayers(_wallLayer);
            if (isTouchingWall && IsAbilityUnlocked(PlayerAbilities.WallJump))
            {
                _state = PlayerState.WallJump;
            }
            else if (_jumpsLeft <= 0 || !IsAbilityUnlocked(PlayerAbilities.DoubleJump))
            {
                return;
            }
            else
            {
                _jumpsLeft--;
            }
        }
        
        _rigidbody.velocity = new Vector2(_moveSpeed * _horizontalInput, _jumpSpeed);
        _animator.SetTrigger(Jump);
    }

    private void OnDash()
    {
        if (_state != PlayerState.Move || !IsAbilityUnlocked(PlayerAbilities.Dash))
        {
            return;
        }

        if (!_isOnGround)
        {
            if (_dashesLeft == 0)
            {
                return;
            }
            else
            {
                _dashesLeft--;
            }
        }
        _animator.SetTrigger(Dash);
        _state = PlayerState.Dash;
    }

    private void OnAttack()
    {
        if (_state == PlayerState.Dash || _state == PlayerState.Locked)
        {
            return;
        }

        _animator.SetTrigger(Attack);
        _animator.SetInteger(AttackType, _attackType);
        _attackType = (_attackType + 1) % AttackTypesAmount;
        _hit.Play();
    }

    private void OnInteract()
    {
        if (_state == PlayerState.Locked)
        {
            return;
        }
        foreach (IInteractable interactable in _activeInteracts)
        {
            interactable.Interact(gameObject);
        }
    }
}
