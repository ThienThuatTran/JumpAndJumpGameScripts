using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerHK : MonoBehaviour
{
    [Header("Check ground")]
    [SerializeField] LayerMask platformLayerMask;
    [SerializeField] LayerMask movingPlatformLayerMask;
    private bool isGrounded = false;

    [Header("Movement")]
    [SerializeField] private float startingSpeed = 15f;
    private float speed = 10f;
    private bool canControlMovement = true;
    private float inputHorizontalMovement = 0;
    public float towardRight = 1f;
    private Vector2 moveDirection = new Vector2(0, 0);

    [Header("Jump")]
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float jumpSpeedFactor = 1.4f;
    private bool canEnemyJump = false;
    private bool isJumping = false;

    [Header("Double Jump")]
    [SerializeField] private int airJumpCounter = 1;


    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferTime = 0.2f;
    private float jumpBufferTimer = -1;

    [Header("Fall Gravity")]
    private float startingGravityScale = 5;
    [SerializeField] private float gravityFactorWhenFall = 1.3f;
    [SerializeField] private float gravityFallHoldJump = 0.8f;

    [Header("Dash")]
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashingTimer = 0.3f;
    private bool isDashing = false;
    private int airDashCounter = 1;

    [Header("Dash Buffer")]
    [SerializeField] private float dashBufferTime = 0.2f;
    private float dashBufferTimer = -1;

    [Header("Hold Jump")]
    [SerializeField] private float slightJumpForceFactor = 0.5f;
    [SerializeField] private float jumpPressTime = 0.2f;
    private bool isFalling = false;
    private float jumpPressTimer = 0;
    //[SerializeField] private float timeToJumpStable = 0.15f;
    //private bool allowHoldJump = false;


    [Header("Grab")]
    private bool isGrabing = false;
    private bool isFirstSliding = false;
    private bool haveWallSlideInputInFirst = false;

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpBufferTime = 0.1f;
    private float wallJumpBufferTimer = -1;
    private int wallDir;
    [Header("One Way Collision")]
    [SerializeField] private float oneWayWaitTime = 0.2f;
    private float oneWayWaitTimer = -1;

    [Header("Animator")]
    [SerializeField] Animator playerAnimator;


    private Rigidbody2D playerRb2D;
    private PlayerInputAction playerInputAction;
    private BoxCollider2D playerCollider;
    [SerializeField] private BoxCollider2D childCollider;
    private Transform playerTrasform;

    private PlatformEffector2D platformEffector;

    private Collider2D groundCheckCollider;
    private TrailRenderer trailRenderer;

    [Header("Accelerate")]
    [SerializeField] private float accelerationTime = 1f;
    [SerializeField] private float startingXVelocityInGround = 0.3f;
    [SerializeField] private float startingXVelocityInAir = 0.1f;
    private float startingHorizontalVelocity = 0.2f;
    private float accelerationTimer = -1;
    private bool isAccelerating = false;
    private float inputHorizontalInLastFrame = 0;

    [Header("Deceleration")]
    [SerializeField] private float decelerationTime = 0.3f;
    private float decelerationTimer = -1;
    private bool isDeceleration = false;

    private bool isFirstDoubleJump;
    private int firstDoubleJumpCount = 0;

    [Header("Floating")]
    private bool isFloating;
    [SerializeField] private float minusYVelocityValue = 0.25f;
    [SerializeField] private float floatTime = 0.1f;
    private float floatTimer = -1;
    [SerializeField] private float graivityWhenFloat = 1;
    private bool canFloat = false;


    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimer = -1;

    [Header("KnockBack")]
    private float knockbackDuration;
    [SerializeField] private Vector2 knockbackForce;
    [SerializeField] private FlashDamageEffect damageFX;

    [Header("Hit box")]
    [SerializeField] private Transform footCheck;
    [SerializeField] private Vector2 enemyCheckBoxSize = new Vector2(1, 0.5f);
    [SerializeField] private LayerMask enemyLayerMask;

    

    [Header("Edge Escape")]
    [SerializeField] private Vector2 escapeDirection = new Vector2();
    [SerializeField] private float platformColliCheckDistance = 0.5f;
    private bool canEdgeEscape = false;
    [SerializeField] private float escapeEdgeSpeed = 10f;
    private Coroutine escapeEdgeCoroutine;



    private Vector3 lastMovingPlatformPosition = Vector3.zero;
    private bool isFirstOnMovingP = true;

    private bool isFirstGrapplingOnMoveP = true;
    private Vector3 lastGrapplingMovingPPos = Vector3.zero;
    //private bool isPreventFloating = false;

    private bool isInTheAir;

    [Header("Camera Look")]
    private bool isFirstLookUp = true;
    private bool isFirstLookDown = true;

    private void Awake()
    {

        PlayerStatus.Instance.GetPlayerHealth().OnDead += PlayerStatus_OnDead;

        //startingGravityScale = playerRb2D.gravityScale;

        playerInputAction = InputManager.Instance.GetPlayerInputAction();
        InputManager.Instance.SetPlayerMapsInputAction();
        //playerInputAction.Player.Enable();
        playerInputAction.Player.Jump.performed += Jump;
        playerInputAction.Player.Dash.performed += Dashing;
        playerInputAction.Player.OneWayCollision.performed += OneWayCollision_performed;

        playerInputAction.Player.MenuOpen.performed += MenuOpen_performed;

        playerInputAction.Player.LookDown.performed += LookDown_performed;
        playerInputAction.Player.LookUp.performed += LookUp_performed;

        playerInputAction.Player.LookDown.canceled += LookDown_canceled;
        playerInputAction.Player.LookUp.canceled += LookUp_canceled;
        playerInputAction.UI.MenuClose.performed += MenuClose_performed;

    }

    private void MenuClose_performed(InputAction.CallbackContext obj)
    {
        GameManager.Instance.ResumeGame();
    }

    private void LookUp_canceled(InputAction.CallbackContext obj)
    {
        CameraManager.Instance.ReturnNormalCamera();
    }

    private void LookDown_canceled(InputAction.CallbackContext obj)
    {
        CameraManager.Instance.ReturnNormalCamera();
    }

    private void LookUp_performed(InputAction.CallbackContext obj)
    {
        CameraManager.Instance.LookUp();
    }

    private void LookDown_performed(InputAction.CallbackContext obj)
    {
        CameraManager.Instance.LookDown();
        
    }

    private void MenuOpen_performed(InputAction.CallbackContext obj)
    {
        GameManager.Instance.PauseGame();
    }

    private void PlayerStatus_OnDead(object sender, EventArgs e)
    {
        Die();
    }


    private void Start()
    {
        playerRb2D = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerTrasform = GetComponent<Transform>();
        trailRenderer = GetComponent<TrailRenderer>();

        trailRenderer.emitting = false;

        knockbackDuration = PlayerStatus.Instance.GetKnockTime();

        startingGravityScale = playerRb2D.gravityScale;
    }

    private void OneWayCollision_performed(InputAction.CallbackContext obj)
    {
        if (oneWayWaitTimer < 0)
        {
            try { platformEffector = IsGrounded().GetComponent<PlatformEffector2D>(); } catch { }
            if (platformEffector != null) oneWayWaitTimer = oneWayWaitTime;
        }

    }

    private void Update()
    {
        if (PlayerStatus.Instance.GetIsKnocked() || PlayerStatus.Instance.GetIsDead())
        {
            ResetBufferTimer();

            return;
        }
        
        isGrounded = IsGrounded() != null;
        PlayerStatus.Instance.isGrounded = isGrounded;

        wallDir = WallDirection();

        HandleAnimations(); 
        if (isGrounded)
        {
            if (canEnemyJump)
            {
                canEnemyJump = false;
            }
            HandleGrounded();
        }
        else
        {
            InTheAir();
        }

        if (wallDir != 0)
        {
            OnTheWall();
        }

        if (wallDir == 0 || wallDir * towardRight <= 0)
        {
            wallJumpBufferTimer -= Time.deltaTime;
        }

        if (isFloating)
        {
            floatTimer -= Time.deltaTime;
            if (floatTimer < 0) { isFloating = false; }
        }

        if (isAccelerating)
        {
            accelerationTimer -= Time.deltaTime;
            if (accelerationTimer < 0) { isAccelerating = false; }
        }
        decelerationTimer -= Time.deltaTime;

        HandleOneWayCollision();

        FlipCharacter();
    }

    private void ResetBufferTimer()
    {
        wallJumpBufferTimer = -1;
        jumpBufferTimer = -1;
        dashBufferTimer = -1;
    }

    private void HandleAnimations()
    {
        playerAnimator.SetFloat("xVelocity", playerRb2D.linearVelocityX);
        playerAnimator.SetFloat("yVelocity", playerRb2D.linearVelocityY);
        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetBool("isWallDetected", isGrabing);
    }
    private void HandleGrounded()
    {
        startingHorizontalVelocity = startingXVelocityInGround;
        canFloat = true;
        if (AtTheEdge()) { childCollider.enabled = false; }
        else { childCollider.enabled = true; }

        playerRb2D.gravityScale = startingGravityScale;
        speed = startingSpeed;

        ResetInGround();

        decelerationTimer = -1;
        coyoteTimer = coyoteTime;
        canEdgeEscape = true;
    }

    private void InTheAir()
    {
        childCollider.enabled = false;
        //isAccelerating = false;
        coyoteTimer -= Time.deltaTime;
        startingHorizontalVelocity = startingXVelocityInAir;
        speed = startingSpeed * jumpSpeedFactor;

        jumpBufferTimer -= Time.deltaTime;
        dashBufferTimer -= Time.deltaTime;
    }
    private void ResetInGround()
    {
        airDashCounter = 1;
        jumpPressTimer = 0;
        airJumpCounter = 1;
        floatTimer = -1;
    }

    private void OnTheWall()
    {
        airDashCounter = 1;
        airJumpCounter = 1;
    }

    private void HandleOneWayCollision()
    {
        if (platformEffector != null)
        {
            if (oneWayWaitTimer > 0)
            {
                platformEffector.rotationalOffset = 180;
                //oneWayWaitTimer = -1;
            }
            else { platformEffector.rotationalOffset = 0; }
        }


        if (oneWayWaitTimer > -1) oneWayWaitTimer -= Time.deltaTime;
    }

    private void FlipCharacter()
    {
        transform.localScale = new Vector3(towardRight, 1, 1);
    }
    private void HandleHoldJump()
    {
        if (playerInputAction.Player.Jump.ReadValue<float>() > Mathf.Epsilon)
        {
            jumpPressTimer += Time.deltaTime;
        }
        else
        {

            if (isJumping)
            {
                /*
                if (jumpPressTimer < jumpPressTime)
                {
                    playerRb2D.linearVelocity = new Vector2(playerRb2D.linearVelocity.x, 0);
                }*/
                playerRb2D.linearVelocity = new Vector2(playerRb2D.linearVelocity.x, 0);
                isJumping = false;
                canFloat = false;
            }

        }
    }
    private void HandleWallOnTop()
    {
        if (WallOnTop())
        {
            playerRb2D.linearVelocityY = 0;
        }
    }

    private void FixedUpdate()
    {
        if (PlayerStatus.Instance.GetIsKnocked() || PlayerStatus.Instance.GetIsDead())
        {

            return;
        }

        HandleHoldJump();
        if (PlayerStatus.Instance.CanWallJump())
        {
            HandleGrab();
        }
        //HandleWallOnTop();
        HandleJumpBuffer();

        if (PlayerStatus.Instance.CanDash())
        {
            HandleDashBuffer();
        }

        HandleMovement();

        if (!isGrounded) HandleHitBox();
        //Floating();
        FallGravity();

    }

    private void HandleJumpBuffer()
    {
        if ((isGrounded && jumpBufferTimer > 0 && !isJumping) || (canEnemyJump && jumpBufferTimer > 0))
        {

            ProcessJump();
            if (canEnemyJump)
            {
                canEnemyJump = false;
            }
            jumpBufferTimer = -1;
        }
    }

    private void HandleFloating()
    {
        if (isDashing || isGrabing) { return; }
        if (canFloat && !isFloating && !isGrounded)
            if (Mathf.Abs(playerRb2D.linearVelocity.y) < minusYVelocityValue || (MathF.Abs(inputHorizontalMovement) > 0.1f) && !isJumping)
            {
                canFloat = false;
                isFloating = true;
                floatTimer = floatTime;
            }
    }
    private void Floating()
    {
        if (isFloating)
        {
            playerRb2D.gravityScale = graivityWhenFloat;
        }
    }

    private void FallGravity()
    {
        if (isDashing) { playerRb2D.gravityScale = 0; return; } //playerRb2D.gravityScale = startingGravityScale; }
        if (isFloating) { return; }
        if (playerRb2D.linearVelocityY < 0f)
        {
            if (isGrabing) return;
            //playerAnimator.SetBool("IsJumping", false);
            //playerAnimator.SetBool("IsFalling", true);
            isJumping = false;
            float fallHeight = FallHeight();

            float sGravity = 1f;
            float mGravity = 1.3f;
            float lGravity = 1.5f;

            if (fallHeight > 10f)
            {
                playerRb2D.gravityScale = startingGravityScale * sGravity;
            }
            else if (fallHeight > 3f){
                playerRb2D.gravityScale = startingGravityScale * mGravity;
            }
            else if (fallHeight > 0.5f)
            {
                playerRb2D.gravityScale = startingGravityScale * lGravity;
            }
        }
    }



    private void HandleMovement()
    {
        
        inputHorizontalMovement = playerInputAction.Player.Movement.ReadValue<Vector2>().x;
        
        moveDirection.x = inputHorizontalMovement;

        if (Mathf.Abs(inputHorizontalMovement) > Mathf.Epsilon)
        {
            if (Mathf.Abs(inputHorizontalInLastFrame) < Mathf.Epsilon)
            {
                isAccelerating = true;
                accelerationTimer = accelerationTime;
            }
        }
        else
        {
            isAccelerating = false;
        }

        if (!isGrounded && Mathf.Abs(inputHorizontalMovement) < Mathf.Epsilon)
        {
            if (Mathf.Abs(inputHorizontalInLastFrame) > Mathf.Epsilon)
            {
                decelerationTimer = decelerationTime;
            }
        }

        inputHorizontalInLastFrame = inputHorizontalMovement;

        if (isDashing || !canControlMovement)
        {
            isAccelerating = false;
            return;
        }

        if (inputHorizontalMovement < -Mathf.Epsilon) { towardRight = -1; }
        else if (inputHorizontalMovement > Mathf.Epsilon) { towardRight = 1; }


        if (isGrabing && wallDir * inputHorizontalMovement > 0 || wallJumpBufferTimer>0)
        {
            return;
        }

        if (isGrounded)
        {
            playerRb2D.linearVelocity = new Vector2(inputHorizontalMovement * speed, playerRb2D.linearVelocity.y);
        }
        else
        {
            playerRb2D.linearVelocity = new Vector2(inputHorizontalMovement * speed * 0.9f, playerRb2D.linearVelocity.y);
        }

        

        if (Mathf.Abs(inputHorizontalMovement) > Mathf.Epsilon)
        {
            Debug.Log("movementInput");
            if (isAccelerating)
            {
                if (isGrounded)
                {
                    float accelerationVelocity = (1 - accelerationTimer / accelerationTime + startingHorizontalVelocity) * startingSpeed;
                    if (accelerationVelocity > startingSpeed) { accelerationVelocity = startingSpeed; }

                    playerRb2D.linearVelocity = new Vector2(inputHorizontalMovement * accelerationVelocity, playerRb2D.linearVelocity.y);
                }
                else
                {
                    playerRb2D.linearVelocity = new Vector2(inputHorizontalMovement * speed, playerRb2D.linearVelocity.y);
                }

            }

            else
            {
                Debug.Log("movement");
                playerRb2D.linearVelocity = new Vector2(inputHorizontalMovement * speed, playerRb2D.linearVelocity.y);
            }
                
        }
        else
        {
            if (decelerationTimer > 0 && !isGrounded)
            {
                float decelerationVelocity = (decelerationTimer / decelerationTime) * speed;
                playerRb2D.linearVelocity = new Vector2(towardRight * decelerationVelocity, playerRb2D.linearVelocity.y);
            }

        }
        HandleMovementOnMovingPlatform();

    }

    private void HandleMovementOnMovingPlatform()
    {
        Collider2D movingPlatform = GetMovingPlatform();
        if (movingPlatform != null)
        {

            Vector3 currentMovingPlatformPosition = movingPlatform.gameObject.transform.position;

            if (!isFirstOnMovingP) transform.position += currentMovingPlatformPosition - lastMovingPlatformPosition;
            if (isFirstOnMovingP) { isFirstOnMovingP = false; }
            lastMovingPlatformPosition = currentMovingPlatformPosition;
        }
        else
        {
            isFirstOnMovingP = true;
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        bool onGround = IsGrounded() != null;
        int wallDirect = WallDirection();
        isFloating = false;
        playerRb2D.gravityScale = startingGravityScale;

        jumpBufferTimer = jumpBufferTime;
        if (!onGround)
        {
            if (!isJumping && coyoteTimer > 0) { ProcessJump(); coyoteTimer = -1; return; }

            if (wallJumpBufferTimer > 0 && PlayerStatus.Instance.CanWallJump())
            {
                wallJumpBufferTimer = -1;
                HandleWallJump(wallDirect);
            }
            else if (wallDirect == 0)
                if (CanDoubleJump() && PlayerStatus.Instance.CanDoubleJump() && !canEnemyJump)
                {
                    HandleDoubleJump();
                }
        }
    }

    private void ProcessJump()
    {

        playerRb2D.linearVelocity = new Vector2(playerRb2D.linearVelocity.x * 0.9f, jumpForce);
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.jumpSFX, transform, 0.6f);
        isJumping = true;
    }
    private Collider2D IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, 0.3f, platformLayerMask);
        return raycastHit2D.collider;
    }

    private bool WallOnTop()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.up, 0.1f, platformLayerMask);
        return raycastHit2D.collider != null;
    }

    private bool AtTheEdge()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + 0.3f, platformLayerMask);
        return raycastHit.collider != null;
    }

    private int WallDirection()
    {
        int wallDirection = 0;

        RaycastHit2D raycastHit2Dleft = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.left, 0.1f, platformLayerMask);
        RaycastHit2D raycastHit2DRight = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.right, 0.1f, platformLayerMask);

        if (raycastHit2Dleft.collider != null ) { wallDirection = -1; }
        else if (raycastHit2DRight.collider != null) { wallDirection = 1; }
        else { wallDirection = 0; }
        return wallDirection;
    }
    private float FallHeight()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, float.MaxValue, platformLayerMask);
        if (raycastHit2D.collider != null )
        {
            return raycastHit2D.distance;
        }
        else
        {
            return 0f;
        }
    }
    private void Dashing(InputAction.CallbackContext callbackContext)
    {
        dashBufferTimer = dashBufferTime;
    }
    private void HandleDashing()
    {

        if (isDashing) return;
        trailRenderer.emitting = true;
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.dashSFX, transform,1f);
        if (towardRight == 1)
        {
            playerRb2D.linearVelocity = new Vector2(dashForce, playerRb2D.linearVelocity.y * 0);
        }
        else
        {
            playerRb2D.linearVelocity = new Vector2(-dashForce, playerRb2D.linearVelocity.y * 0);
        }

        airDashCounter--;
        isDashing = true;
        Invoke(nameof(this.PreventDash), dashingTimer);
    }

    private void PreventDash()
    {
        isDashing = false;
        trailRenderer.emitting = false;
        playerRb2D.gravityScale = startingGravityScale;
        playerRb2D.linearVelocity = new Vector2(0, 0 * playerRb2D.linearVelocity.y);
    }
    private void HandleDashBuffer()
    {
        if (dashBufferTimer > 0 && CanDash())
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

        if (airJumpCounter < 1) return false;
        else return true;
    }

    private void HandleDoubleJump()
    {
        firstDoubleJumpCount++;
        //playerRb2D.velocity = new Vector2(playerRb2D.velocity.x , jumpForce * 0.9f);
        ProcessJump();
        //speed = 1.2f * speed;
        airJumpCounter--;
    }
    private void HandleGrab()
    {

        if (isGrounded || isJumping || wallDir == 0)
        {
            isGrabing = false;
            haveWallSlideInputInFirst = false;
            return;
        }
        
        //haveWallSlideInputInFirst = wallDir * towardRight > 0.5f && (inputHorizontalMovement * (float)towardRight > 0.5f);
        if (wallDir * towardRight > 0.5f && (inputHorizontalMovement * (float)towardRight > 0.5f))
        {
            haveWallSlideInputInFirst = true;
            
        }

        if (haveWallSlideInputInFirst && wallDir * towardRight > 0.5f)
        {
            isGrabing = true;
            wallJumpBufferTimer = wallJumpBufferTime;
            playerRb2D.linearVelocityY = -5f;
        }
    
    }
    private void HandleWallJump(int direction)
    {

        playerRb2D.linearVelocity = new Vector2(-direction * speed *1.5f, jumpForce * 1.3f);

        canControlMovement = false;
        isFirstSliding = false;
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.wallJumpSFX, transform, 0.6f);

        Invoke(nameof(this.AllowControlMovement), 0.1f);
        isJumping = true;
    }
    private void AllowControlMovement()
    {
        canControlMovement = true;
    }

    private Collider2D CheckCollisionWhenJump()
    {
        
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.up, platformColliCheckDistance, platformLayerMask);
        return raycastHit2D.collider;
    }
    private void FakeCollisionEnter2D()
    {
        int towardEdge = DetectPlatform();
        if (towardEdge * towardRight >= 0) return;
        escapeEdgeCoroutine = StartCoroutine(MoveToEscapeEdge(transform.position, -towardEdge));
    }

    private int DetectPlatform()
    {
        int towardPlatform = 0;

        RaycastHit2D raycastHitLeft = Physics2D.Raycast(transform.position - new Vector3(playerCollider.bounds.extents.x, 0, 0),
            Vector2.up, playerCollider.bounds.extents.y + platformColliCheckDistance + 1f, platformLayerMask);
        RaycastHit2D raycastHitRight = Physics2D.Raycast(transform.position + new Vector3(playerCollider.bounds.extents.x, 0, 0),
            Vector2.up, playerCollider.bounds.extents.y + platformColliCheckDistance + 1f, platformLayerMask);

        if (raycastHitLeft.collider != null && raycastHitRight.collider != null) { return 0; }
        if (raycastHitLeft.collider != null)
        {
            towardPlatform = -1;
        }
        else if (raycastHitRight.collider != null)
        {
            towardPlatform = 1;
        }

        return towardPlatform;
    }

    private IEnumerator MoveToEscapeEdge(Vector3 startPosition, int direction)
    {
        Vector3 destinatePosition = startPosition + new Vector3(playerCollider.bounds.extents.x * 0.8f, 0, 0) * direction;
        while (Vector3.Distance(transform.position, destinatePosition) > 0.05f)
        {

            transform.position = Vector3.MoveTowards(transform.position, destinatePosition, escapeEdgeSpeed * Time.deltaTime);
            yield return null;
        }
        //trailRenderer.emitting = false;

    }

    private Collider2D GetMovingPlatform()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(footCheck.position, new Vector2(playerCollider.bounds.size.x *0.5f, 0.4f), 0f, Vector2.down, 0.2f, movingPlatformLayerMask);
        return raycastHit2D.collider;
    }

    

    public void Knockback(float sourceDamageXPosition)
    {
        PlayerStatus.Instance.SetIsKnocked(true);

        playerCollider.excludeLayers = enemyLayerMask;

        playerRb2D.linearVelocity = Vector2.zero;

        float damageDir = 1;
        if (sourceDamageXPosition > transform.position.x)
        {
            damageDir = -1;
        }
        
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.hurtSFX, transform, 0.6f);

        StartCoroutine(KnockbackRoutine());

        playerRb2D.linearVelocity = new Vector2(knockbackForce.x * damageDir, knockbackForce.y);
    }
    private IEnumerator KnockbackRoutine()
    {
        damageFX.CallDamageFlash();
        PlayerStatus.Instance.GetPlayerHealth().Damage(1);

        yield return new WaitForSeconds(knockbackDuration);

        
        playerCollider.excludeLayers = 0;
        PlayerStatus.Instance.SetIsKnocked(false);
    }

    public void InDeadZone()
    {
        if (!GameManager.Instance.isTutorialLevel)
        {
            PlayerStatus.Instance.GetPlayerHealth().Damage(1);
        }
        
        if (!PlayerStatus.Instance.GetIsDead())
        {
            SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.fallingSFX, transform, 0.5f);
            UI_FadeEffect.Instance.ScreenFade(1, 1, false, Respawn);
        }
        
        
    }
    private void Respawn()
    {
        GameManager.Instance.RespawnPlayer();
    }

    private void HandleHitBox()
    {
        if (isJumping)
        {
            return;
        }

        Collider2D[] enemyColliders = Physics2D.OverlapBoxAll(footCheck.transform.position, enemyCheckBoxSize, 0, enemyLayerMask);

        foreach (Collider2D enemyCollider in enemyColliders)
        {
            
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                ScreenShake.Instance.EnemyDefeatScreenShake();
                SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.hitSFX, transform, 0.6f);

                enemy.Die();

                playerRb2D.linearVelocity = new Vector2(playerRb2D.linearVelocity.x * 0.8f, jumpForce);
                canEnemyJump = true;
            }
        }
    }

    public void Die()
    {
        playerCollider.enabled = false;
        playerRb2D.gravityScale = 0;
        playerRb2D.linearVelocity = Vector2.zero;
        playerRb2D.linearVelocityY = 4;

        playerAnimator.SetTrigger("die");

        UI_FadeEffect.Instance.ScreenFade(1, 2, false, GameManager.Instance.GameOver);
        PlayerStatus.Instance.SetIsDead();
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.deathSFX, transform, 0.6f);


    }

    private void OnEnable()
    {
        PlayerStatus.Instance.SetIsKnocked(false);

        canControlMovement = true;
    }

    private void OnDisable()
    {
        damageFX.CancelDamageFX();
    }

}
