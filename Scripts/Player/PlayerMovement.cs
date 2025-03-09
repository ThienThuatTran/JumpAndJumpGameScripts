using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] LayerMask platformLayerMask;
    [SerializeField] private float speed = 10f;

    [SerializeField] private float slightJumpForceFactor = 0.5f;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float startingGravityScale;
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashingTimer = 0.3f;
    [SerializeField] private float startingGravityWhenFall = 2f;

    [SerializeField] private float wallJumpBufferTime = 0.2f;
    private float wallJumpBufferTimer = -1;

    private float gravityWhenFall = 1f;

    private float startingSpeed = 15f;
    private Rigidbody2D rigibody;
    private PlayerInputAction playerInputAction;
    private CapsuleCollider2D boxCollider;
    private Transform playerTrasform;
    private bool isDashing = false;
    private float towardRight = 1f;
    private int airDashCounter = 1;
    private int airJumpCounter = 1;
    [SerializeField] private float jumpPressTime = 0.2f;
    private float jumpPressTimer = 0;

    [SerializeField] private float jumpBufferTime = 0.2f;
    private float jumpBufferTimer = -1;

    private bool canControlMovement = true;
    private bool isGrounded = true;
    private bool isJumping = false;
    private bool canGrab = true;
    private float inputHorizontalMovement = 0;
    private float inputVerticalMovement = 0;
    private int wallDir;
    private bool isGrabing =false;

    private float dashBufferTime = 0.2f;
    private float dashBufferTimer = -1;

    private void Awake()
    {
        //Application.targetFrameRate = 60;
        rigibody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<CapsuleCollider2D>();
        playerTrasform = GetComponent<Transform>();
        
        startingGravityScale = rigibody.gravityScale;
        playerInputAction = new PlayerInputAction();

        playerInputAction.Player.Enable();
        playerInputAction.Player.Jump.performed += Jump;

        playerInputAction.Player.Movement.performed += Movement_performed;

        playerInputAction.Player.Dash.performed += Dashing;
        playerInputAction.Player.Grab.started += Grab;
    }

    private void Movement_performed(InputAction.CallbackContext callbackContext)
    {
        //Debug.Log(callbackContext);
    }
    private void Update()
    {
        //Debug.Log(isGrabing);
        if (IsGrounded())
        {
            speed = startingSpeed;
            airDashCounter = 1;
            //jumpBufferTimer = -1;
            jumpPressTimer = 0;
            airJumpCounter = 1;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
            dashBufferTimer -= Time.deltaTime;
            
        }
        if (IsWall()!=0) 
        {
            wallJumpBufferTimer = wallJumpBufferTime;
        }
        else
        {
            wallJumpBufferTimer -= Time.deltaTime;
        }

        HandleHoldJump();
        //Debug.Log(Time.deltaTime);
        FlipCharacter();
        Debug.Log(towardRight);
    }
    private void FlipCharacter()
    {
        transform.localScale = new Vector3(towardRight,1,1);
        Debug.Log(transform.position);
    }
    private void HandleHoldJump()
    {
        if (playerInputAction.Player.Jump.ReadValue<float>() > Mathf.Epsilon) { jumpPressTimer += Time.deltaTime; }
        else if (isJumping)
        {
            if (jumpPressTimer < jumpPressTime)
            {
                //Debug.Log(jumpPressTimer);
                rigibody.linearVelocity = rigibody.linearVelocity * slightJumpForceFactor;
            }
            isJumping = false;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        //isWall = IsWall();
        wallDir = IsWall();
        HandleJumpBuffer();
        HandleDashBuffer();
        HandleMovement();
        HandleGrab();
        FallGravity();
    }

    private void HandleJumpBuffer()
    {
        if ((isGrounded && jumpBufferTimer > 0))
        {
            ProcessJump();
            jumpBufferTimer = -1;
        }
    }

    private void FallGravity()
    {
        if (rigibody.linearVelocity.y < 0f)
        {
            //if (gravityWhenFall == startingGravityWhenFall) { Debug.Log("Max Height" + transform.localPosition.y); }
            if (rigibody.gravityScale != startingGravityScale) { rigibody.gravityScale = startingGravityScale; }
            rigibody.gravityScale *= gravityWhenFall;
            gravityWhenFall = 1;
            
        }
        else if (wallDir == 0)
        { 
            rigibody.gravityScale = startingGravityScale;
            gravityWhenFall = startingGravityWhenFall;
        }
    }

    private void HandleMovement()
    {
        inputHorizontalMovement = playerInputAction.Player.Movement.ReadValue<Vector2>().x;
        inputVerticalMovement = playerInputAction.Player.Movement.ReadValue<Vector2>().y;
        if (isDashing) { return; }
        if (isGrabing) { return; }
        if (!canControlMovement) { return; }

        
        if (inputHorizontalMovement < -Mathf.Epsilon) { towardRight = -1; } 
        else if (inputHorizontalMovement >Mathf.Epsilon) { towardRight =  1; }
        if (inputHorizontalMovement != 0 || isGrounded) rigibody.linearVelocity = new Vector2(inputHorizontalMovement * speed, rigibody.linearVelocity.y);
        if (!isGrounded && inputHorizontalMovement ==0) rigibody.linearVelocity = new Vector2(rigibody.linearVelocity.x*0.94f, rigibody.linearVelocity.y);
    }

    public void Jump(InputAction.CallbackContext callbackContext )
    {
        bool isGround = IsGrounded();
        int wallDirect = IsWall();
        jumpBufferTimer = jumpBufferTime;
        if (isGround)
        {
            //ProcessJump();
        }
        else
        {

            if (wallDirect ==0) if (CanDoubleJump()) HandleDoubleJump();
        }
        if (wallJumpBufferTimer > 0 && !isGround) { Debug.Log(wallDirect); HandleWallJump(); }
    }
    private void ProcessJump()
    {
        rigibody.linearVelocity = Vector2.up * jumpForce;
        isJumping = true;
    }
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.CapsuleCast(boxCollider.bounds.center, boxCollider.bounds.size, boxCollider.direction, 0f, Vector2.down, 0.2f, platformLayerMask);
        return raycastHit2D.collider != null;
    }
    private int IsWall()
    {
        int wallDirection = 0;
        RaycastHit2D raycastHit2Dleft = Physics2D.CapsuleCast(boxCollider.bounds.center, boxCollider.bounds.size, boxCollider.direction,0f, Vector2.left, 0.3f, platformLayerMask);
        RaycastHit2D raycastHit2DRight = Physics2D.CapsuleCast(boxCollider.bounds.center, boxCollider.bounds.size, boxCollider.direction, 0f, Vector2.right, 0.3f, platformLayerMask);
        /*
        Color rayColor;
        if (raycastHit2D.collider != null)
        {
            rayColor = Color.green;
        }
        else rayColor = Color.red; 

        Debug.DrawRay(boxCollider.bounds.center + new Vector3(0,boxCollider.bounds.extents.y), Vector2.left * (boxCollider.bounds.extents.x + 0.1f *towardRight), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(0,boxCollider.bounds.extents.y), Vector2.left * (boxCollider.bounds.extents.x + 0.1f * towardRight), rayColor);
        */
        if (raycastHit2Dleft.collider != null) { wallDirection = -1; }
        else if (raycastHit2DRight.collider != null) { wallDirection = 1; }
        else { wallDirection = 0; }
        return wallDirection;
    }
    private void Dashing(InputAction.CallbackContext callbackContext)
    {
        if (!CanDash()) { dashBufferTimer = dashBufferTime; return; }
        HandleDashing();

    }
    private void HandleDashing()
    {
        isDashing = true;

        if (towardRight == 1)
        {
            rigibody.linearVelocity = new Vector2(dashForce, rigibody.linearVelocity.y);
        }
        else
        {
            rigibody.linearVelocity = new Vector2(-dashForce, rigibody.linearVelocity.y);
        }
        airDashCounter--;
        // "HandleDashing" = nameof....
        Invoke(nameof(this.CantDash), dashingTimer);
    }

    private void CantDash()
    {
        isDashing = false;
    }
    private void HandleDashBuffer()
    {
        if (dashBufferTimer > 0 && isGrounded)
        {
            HandleDashing();
            dashBufferTimer = -1;
        }
    }
    
    
    private bool CanDash()
    {
        if (airDashCounter == 0) return false;
        else return true;
    }

    private bool CanDoubleJump()
    {
        if (airJumpCounter == 0) return false;
        else return true;
    }
    
    private void HandleDoubleJump()
    {
        ProcessDoubleJump();
        airJumpCounter--;
    }
    private void ProcessDoubleJump()
    {
        rigibody.linearVelocity = Vector2.up * jumpForce * 0.8f;
        speed = 1.7f * speed;
    }
    private void Grab(InputAction.CallbackContext callbackContext)
    {

    }
    
    private void HandleGrab()
    {
        
        if (wallDir == 0) 
        {
            isGrabing = false;
            rigibody.gravityScale = startingGravityScale;
            return; 
        }
        if (playerInputAction.Player.Grab.ReadValue<float>() > Mathf.Epsilon && canGrab)
        {
            rigibody.linearVelocity = new Vector2(rigibody.linearVelocity.x, speed * inputVerticalMovement);
            rigibody.gravityScale = 0;
            isGrabing = true;
            Debug.Log("hihihihi");
        }
        else 
        {
            isGrabing = false;
            rigibody.gravityScale = startingGravityScale; 
        }
    }
    private void HandleWallJump()
    {
        /*
        if (inputHorizontalMovement > 0.5f)
        {
            rigibody.velocity = new Vector2(inputHorizontalMovement * speed, jumpForce);
        }
        else
        {
            rigibody.velocity = new Vector2(-IsWall() * speed, jumpForce);
            //Debug.Log(towardRight);
        }*/
        int wallDirect = IsWall();
        rigibody.linearVelocity = new Vector2(-wallDirect * speed, jumpForce);
        canControlMovement = false;
        canGrab = false;
        Invoke(nameof(this.CandControlMovement), 0.1f);
        Invoke(nameof(this.CanGrab), 0.1f);
        isJumping = true;
    }
    private void CandControlMovement()
    {
        canControlMovement = true;
    }
    private void CanGrab()
    {
        canGrab = true;
    }
}
