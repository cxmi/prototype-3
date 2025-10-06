using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Controls controls;
    [SerializeField] private bool canJump;
    [SerializeField] private float moveSpeed, jumpStartingVelocity, gravity, fallSpeedMax, jumpHeldTimerMax, jumpPreloadTimerMax, coyoteTimerMax;
    private float jumpHeldTimer, jumpPreloadTimer, coyoteTimer;
    private bool jumping, isGrounded, wasGroundedLastFrame;
    private Vector3 velocity, velocityInput, velocityPhysics;
    [SerializeField] private LayerMask jumpableMask;
    
    
    public void Start()
    {
        controller = GetComponent<CharacterController>();
        controls = GetComponent<Controls>();
    }

    void Update()
    {
        if (controls.JumpTriggered()) //if jump button is pressed
        {
            if (isGrounded) BeginJump(); //regular jump
            else if (coyoteTimer > 0) BeginJump(); //coyote time jump
            else jumpPreloadTimer = jumpPreloadTimerMax; //if you are not grounded and didn't coyote jump, start the jump preload timer
        }
        
        //lower timers at the end of each frame
        jumpPreloadTimer -= Time.deltaTime;
        coyoteTimer -= Time.deltaTime;
    }
    
    void FixedUpdate()
    {
        //isGrounded logic
        isGrounded = RaycastTouchesGround();
        if (isGrounded && !wasGroundedLastFrame) {
            GroundEnter();
        }
        if (!isGrounded && wasGroundedLastFrame) {
            GroundExit();
        }
        wasGroundedLastFrame = isGrounded;
        if(isGrounded && velocityPhysics.y <= 0) velocityPhysics.y = 0;

        //gravity logic
        if (jumping && jumpHeldTimer < jumpHeldTimerMax)
        {
            if (controls.JumpHeld()) jumpHeldTimer += Time.fixedDeltaTime;
            else jumpHeldTimer = jumpHeldTimerMax;
        }
        else
        {
            ApplyGravity();
        }

        //movement logic
        velocityInput = transform.right * controls.MoveInput().x + transform.forward * controls.MoveInput().y; //get input velocity
        if ((Mathf.Abs(velocityInput.x) + Mathf.Abs(velocityInput.y)) >= 2) velocityInput.Normalize(); //normalize diagonal movement if I'm using the keyboard to move
        velocityInput *= moveSpeed; //scale by move speed
        
        velocity = velocityInput + velocityPhysics;
        
        controller.Move(velocity * Time.fixedDeltaTime);
    }

    private void ApplyGravity(float gravityMultiplier = 1f)
    {
        velocityPhysics.y -= gravity * gravityMultiplier * Time.fixedDeltaTime; //apply gravity to the physics y velocity
        if (velocityPhysics.y < -fallSpeedMax) velocityPhysics.y = -fallSpeedMax; //make sure fall speed never exceeds fallSpeedMax
        if(isGrounded && velocityPhysics.y < 0) velocityPhysics.y = 0; //if grounded, reset y velocity to 0
    }

    private void BeginJump()
    {
        if(!canJump) return;
        velocityPhysics.y = jumpStartingVelocity;
        jumpHeldTimer = 0;
        coyoteTimer = 0;
        jumpPreloadTimer = 0;
        jumping = true;
    }
    
    private void EndJump(){
        jumping = false;
    }
    
    /// <summary>
    /// Called when you first start touching the ground
    /// </summary>
    void GroundEnter() {
        if (jumping) EndJump();
        //if I just landed on the platform right after pressing the jump button, let me jump
        if (jumpPreloadTimer > 0) BeginJump();
    }

    /// <summary>
    /// Called when you first stop touching the ground
    /// </summary>
    void GroundExit() {
        coyoteTimer = coyoteTimerMax;
    }
    
    /// <summary>
    /// Cast raycasts from the middle of the player and from 8 corners to test if the player is on the ground
    /// </summary>
    private bool RaycastTouchesGround() {
        float rayLength = controller.height * .6f;

        //test if the middle of the player is touching the ground
        if (RaycastTest(transform.position, rayLength)) return true;

        //test if any of the 8 corners of the player is touching the ground
        float halfWidth = transform.localScale.x * .5f;
        float diagonalWidth = transform.localScale.x * .35f;
            //side tests
        if (RaycastTest(transform.position + (transform.right * halfWidth), rayLength)) return true;
        if (RaycastTest(transform.position + (-transform.right * halfWidth), rayLength)) return true;
        if (RaycastTest(transform.position + (transform.forward * halfWidth), rayLength)) return true;
        if (RaycastTest(transform.position + (-transform.forward * halfWidth), rayLength)) return true;
            //diagonal tests
        if (RaycastTest(transform.position + (transform.right * diagonalWidth) + (transform.forward * diagonalWidth), rayLength)) return true;
        if (RaycastTest(transform.position + (transform.right * diagonalWidth) + (-transform.forward * diagonalWidth), rayLength)) return true;
        if (RaycastTest(transform.position + (-transform.right * diagonalWidth) + (transform.forward * diagonalWidth), rayLength)) return true;
        if (RaycastTest(transform.position + (-transform.right * diagonalWidth) + (-transform.forward * diagonalWidth), rayLength)) return true;
        
        //if nothing is touching the ground, return false
        if(isGrounded) LeaveGroundResults(); //call leave ground results if we were grounded and now we're not
        return false;
    }

    /// <summary>
    /// Test a single raycast to see if it hits the ground
    /// </summary>
    /// <param name="startingPoint"> Where the ray begins (it will cast down from here)</param>
    /// <param name="rayLength">How long the ray casts</param>
    private bool RaycastTest(Vector3 startingPoint, float rayLength) {
        RaycastHit hit;
        if (Physics.Raycast(startingPoint, transform.TransformDirection(Vector3.down), out hit, rayLength, jumpableMask)) {
            OnGroundResults(hit);
            return true;
        }
        else {
            return false;
        }
    }

    private void OnGroundResults(RaycastHit hit)
    {
        //USE THIS IF YOU WANT SOMETHING TO HAPPEN EACH FRAME YOU ARE TOUCHING THE GROUND
    }
    
    private void LeaveGroundResults()
    {
        //USE THIS IF YOU WANT SOMETHING TO HAPPEN EACH TIME YOU LEAVE THE GROUND
    }
}
