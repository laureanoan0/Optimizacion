using UnityEngine;

public class PlayerBehavior: IUpdateable, IFixedUpdateables
{
    private float moveSpeed;
    private float groundDrag;
    private float jumpForce;
    private float jumpCd;
    private float airMult;

    private Transform orientation;
    private Transform transform;

    private CameraBehavior camera;
   
    private float playerHeight;
    private LayerMask groundLayer;

    public bool grounded;
    private bool canJump = true;
    private float jumpTimer = 0f;
    private float hInput;
    private float vInput;
    private Vector3 moveDirection;

    private Rigidbody rb;

    public PlayerBehavior(Transform orientation, Transform playerTransform, Rigidbody rb, PlayerStatsSO stats)
    {
        this.orientation = orientation;
        this.transform = playerTransform;
        this.rb = rb;
        rb.freezeRotation = true;

        moveSpeed = stats.moveSpeed;
        groundDrag = stats.groundDrag;
        jumpForce = stats.jumpForce;
        jumpCd = stats.jumpCd;
        airMult = stats.airMult;
        playerHeight = stats.playerHeight;
        groundLayer = stats.groundLayer;

        camera = new CameraBehavior(transform, orientation);

        UpdateManager.Instance.Register((IUpdateable)this);
        UpdateManager.Instance.Register((IFixedUpdateables)this);
    }
    public void Destroy()
    {
        UpdateManager.Instance.Unregister((IUpdateable)this);
        UpdateManager.Instance.Unregister((IFixedUpdateables)this);
    }

    public void CustomUpdate(float time)
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = groundDrag / 2;

        if (jumpTimer > 0f)
        {
            jumpTimer -= time;
        }
        else canJump = true;

        HandleInput();
    }

    public void CustomFixedUpdate()
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
            ResetJump(jumpCd);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump(float time)
    {
        jumpTimer = time;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * vInput + orientation.right * hInput;

        if (grounded)
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection * moveSpeed * 10f * airMult, ForceMode.Force);
    }
}
