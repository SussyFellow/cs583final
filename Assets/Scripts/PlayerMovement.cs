using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed; //current speed being used, sprint or walk speed
    public float sprintSpeed; 
    public float walkSpeed; 
    public float groundDrag; 
    [Tooltip("Player orientation object goes here")]
    public Transform orientation; 
    public float jumpForce; 
    public float jumpCooldown; 
    public float airMultiplier;
    bool readyToJump;

[Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    [Header("Slope Check")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    bool exitingSlope;



    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        //these variables just need to be initialized here to prevent niche bugs, they get set properly later
        readyToJump = true;
        exitingSlope = false;
        moveSpeed = 0;
    }

    private void Update()
    {
        //check if the player is on the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput(); //gets input and does jumping
        SprintHandler(); //checks if you're sprinting and sets speed accordingly
        SpeedControl(); //makes sure your speed doesnt go too high

        if (grounded) //makes sure you dont slide on the ground but dont slow down in the air
        {
            rb.drag = groundDrag; 
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate() //only use for important continuous physics stuff
    {
        MovePlayer(); //does the normal x and z movement stuff
    }

    private void MyInput() //gets the input for x and z, and jumps if youre ressing the jump button.
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown); //necessary to prevent bugged behavior for jumping extremely rapidly
        }
    }

    private void MovePlayer() //x & y movement
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);
        }
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl() //caps speed as appropriate
    {   
        if (OnSlope() && !exitingSlope) //caps overall velocity when on a slope (and not presently jumping off of that slope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else //caps x & z combined velocity
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump() //jumps
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() //sets some bools, exists to be called on a cooldown to make jumping not buggy with slopes or rapid jumps
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool OnSlope() //checks if youre on a slope
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void SprintHandler() //sets movement speed
    {
        if (Input.GetKey(sprintKey))
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }
}
