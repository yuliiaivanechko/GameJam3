using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _jumpSpeed;

    [SerializeField]
    private int _additionalJumps;

    [SerializeField]
    private float _dashSpeedMultiplier;

    [SerializeField]
    private float _dashDuration;


    [SerializeField]
    private Collider2D _bottomCollider;

    [SerializeField]
    private LayerMask _groundLayer;


    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int Jump = Animator.StringToHash("Jump");

    private Rigidbody2D _rigidbody;
    private float _horizontalInput;
    private int _jumpsLeft;
    private bool _isOnGround;
    private bool _isMoving;
    private bool _isDashing;
    private float _dashTime;
    private float _speedMultiplier;
    private Animator _animator;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _horizontalInput = 0.0f;
        _jumpsLeft = _additionalJumps;
        _isOnGround = _bottomCollider.IsTouchingLayers(_groundLayer);
        _animator = GetComponent<Animator>();
        _speedMultiplier = 1.0f;
        _isDashing = false;
        _isMoving = false;
        _dashTime = 0.0f;

    }

    private void FixedUpdate()
    {

        float velocityY = _rigidbody.velocity.y;
        if (_isDashing)
        {
            velocityY = 0.0f;
        }
        _rigidbody.velocity = new Vector2(_speedMultiplier * _moveSpeed * _horizontalInput, velocityY);
        _animator.SetBool(IsMoving, _isMoving);

        UpdateMovingState();
        UpdateJumpState();
        UpdateDashState(Time.fixedDeltaTime);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void UpdateMovingState()
    {
        _isMoving = _rigidbody.velocity.x != 0;
    }

    private void UpdateJumpState()
    {
        _isOnGround = _bottomCollider.IsTouchingLayers(_groundLayer);
        if (_isOnGround)
        {
            _jumpsLeft = _additionalJumps;
        }
    }

    private void UpdateDashState(float timeDelta)
    {
        if (!_isDashing)
        {
            return;
        }

        _dashTime += timeDelta;
        if (_dashTime > _dashDuration)
        {
            _speedMultiplier = 1.0f;
            _dashTime = 0.0f;
            _isDashing = false;
        }

    }

    private void OnMove(InputValue value)
    {
        if (_isDashing)
        {
            return;
        }
        _horizontalInput = value.Get<float>();
        Vector3 localScale = transform.localScale;
        if (_horizontalInput > 0)
        {
            localScale.x = 1;
        }
        else if (_horizontalInput < 0)
        {
            localScale.x = -1;
        }
        transform.localScale = localScale;
    }

    private void OnJump()
    {
        if (_isDashing)
        {
            return;
        }

        if (!_isOnGround)
        {
            if (_jumpsLeft <= 0)
            {
                return;
            }
            else
            {
                _jumpsLeft--;
            }
        }
        

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpSpeed);
        _animator.SetTrigger(Jump);
    }

    private void OnDash()
    {
        if (_isDashing || !_isMoving)
        {
            return;
        }
        _speedMultiplier = _dashSpeedMultiplier;
        _isDashing = true;
    }


}
