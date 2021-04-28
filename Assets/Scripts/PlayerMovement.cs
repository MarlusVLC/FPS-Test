using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float speed = 12f;
        [Header("Jump")]
        [SerializeField] Transform groundChecker;
        [SerializeField] float groundDistance = 0.4f;
        [SerializeField] float jumpHeight = 3f;
        [SerializeField] LayerMask groundMask;
        [SerializeField] private float mass;

        private CharacterController _controller;
        private Vector3 _velocity;
        private float _gravity;
        private bool _isGrounded;

        
        

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            _gravity = Physics.gravity.y;
        }

        private void Update()
        {
            InputMove();
            FreeFall();
        }

        private void FreeFall()
        {
            _isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
            
            _velocity.y += _gravity * Time.deltaTime;

            _controller.Move(_velocity * Time.deltaTime);
        }

        private void Jump()
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * _gravity * (1/mass));

        }

        private void InputMove()
        {
                        float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            _controller.Move(move * (speed * Time.deltaTime));

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                Jump();
            }
        }


    }
}

