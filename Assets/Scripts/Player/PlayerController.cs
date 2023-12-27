using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public enum PlayerState
    {
        Idle, 
        Move, 
        Dash,
        WallJump
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
    private LayerMask _groundLayer;

    [SerializeField]
    private Collider2D _sidesCollider;

    [SerializeField]
    private LayerMask _wallLayer;

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

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _horizontalInput = 0.0f;
        _jumpsLeft = 1;
        _dashesLeft = 1;
        _isOnGround = _bottomCollider.IsTouchingLayers(_groundLayer);
        _animator = GetComponent<Animator>();
        _dashTime = 0.0f;
        _wallJumpTime = 0.0f;
        _state = PlayerState.Idle;
        _attackType = 0;

    }

    private void FixedUpdate()
    {
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
            localScale.x = 1.0f;
        }
        else if (_rigidbody.velocity.x < 0.0f)
        {
            localScale.x = -1.0f;
        }
        transform.localScale = localScale;

        UpdateMovingState();
        UpdateJumpState();
        UpdateDashState(Time.fixedDeltaTime);
        UpdateWallJumpState(Time.fixedDeltaTime);
        UpdateComboState(Time.fixedDeltaTime);
    }

    private bool IsAbilityUnlocked(PlayerAbilities ability)
    {
        return true;
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
        _isOnGround = _bottomCollider.IsTouchingLayers(_groundLayer);
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
        if (_state == PlayerState.Dash)
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
        if (_state == PlayerState.Dash)
        {
            return;
        }

        _attackType = (_attackType + 1) % AttackTypesAmount;
        _animator.SetTrigger(Attack);
        _animator.SetInteger(AttackType, _attackType);
    }


}
