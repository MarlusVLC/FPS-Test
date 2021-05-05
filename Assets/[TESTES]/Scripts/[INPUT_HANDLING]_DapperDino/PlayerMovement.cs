using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 10f;

    private PlayerInput _playerInput;
    private Rigidbody _rb;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Jump();
    }


    private void Move()
    {
        Vector2 movement = _playerInput.movementInput;
        movement.Normalize();
        movement *= speed * Time.deltaTime;
        
        transform.Translate(movement.x, 0f, movement.y);
    }

    private void Jump()
    {
        if (_playerInput.jumpInput)
             _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse );
    }
}
