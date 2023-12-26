using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float _moveSpeed;

    private PlayerInput _playerInput;
    private Rigidbody2D _rigidbody;
    private float _horizontalInput;
    // Start is called before the first frame update
    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _horizontalInput = 0.0f;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_moveSpeed * _horizontalInput, 0.0f);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnMove(InputValue value)
    {
        _horizontalInput = value.Get<float>();
    }

    private void OnJump()
    {

    }
}
