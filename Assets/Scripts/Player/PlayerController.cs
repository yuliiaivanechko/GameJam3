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
    private Collider2D _bottomCollider;

    [SerializeField]
    private LayerMask _groundLayer;

    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int LookingLeft = Animator.StringToHash("LookingLeft");


    private Rigidbody2D _rigidbody;
    private float _horizontalInput;
    private int _jumpsLeft;
    private bool _isOnGround;
    private Animator _animator;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _horizontalInput = 0.0f;
        _jumpsLeft = _additionalJumps;
        _isOnGround = _bottomCollider.IsTouchingLayers(_groundLayer);
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_moveSpeed * _horizontalInput, _rigidbody.velocity.y);
        bool isMoving = _rigidbody.velocity.x != 0;
        _animator.SetBool(IsMoving, isMoving);
        if (isMoving)
        {
            _animator.SetBool(LookingLeft, _rigidbody.velocity.x < 0);
        }
        UpdateJumpState();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    private void UpdateJumpState()
    {
        _isOnGround = _bottomCollider.IsTouchingLayers(_groundLayer);
        if (_isOnGround)
        {
            _jumpsLeft = _additionalJumps;
        }
    }

    private void OnMove(InputValue value)
    {
        _horizontalInput = value.Get<float>();
    }

    private void OnJump()
    {
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
    }


}
