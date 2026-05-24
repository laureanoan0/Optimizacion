using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform orientation;
    [SerializeField] private float groundDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCd;
    [SerializeField] private float airMult;


    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;

    public bool grounded;
    private bool canJump = true;
    private float hInput;
    private float vInput;
    private Vector3 moveDirection;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        if (grounded) 
            rb.drag = groundDrag;
        else 
            rb.drag = groundDrag /2;

        HandleInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && grounded && canJump)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCd);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * vInput + orientation.right * hInput;

        if(grounded)
            rb.AddForce(moveDirection *  moveSpeed * 10f, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(moveDirection * moveSpeed * 10f * airMult, ForceMode.Force);
    }
}
