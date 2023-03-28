using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleDialogN;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Movement Inputs
    private InputActionsControl input;
    private InputAction i_movement;
    private bool isPressingJump;

    private Rigidbody2D rgb;
    private Vector2 movement;
    private float jumpTimeCounter;
    //[SerializeField]
    private bool isGrounded, isJumping, isWalled, isWallSliding, isWallJumping;
    private int jumpCounter, maxJumps = 1;
    [SerializeField] private float playerVelocity, playerJump, jumpTime;
    [SerializeField] [Range(0.0f,1.0f)] private float playerSlider, playerGlider;
    //[SerializeField]
    private float wallJumpingDirection, wallJumpingTime = 0.2f, wallJumpingCounter, wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(6,6);
    [SerializeField] private float checkGroundLessY, checkGroundLessX, checkGroundRadius, checkWallMoreX, checkWallRadius;
    [SerializeField] private LayerMask groundLayer;
    private float coyoteTime = 0.15f, coyoteTimeCounter;
    public Vector2 getMovement {get{return movement;}}
    public bool getIsGrounded {get{return isGrounded;}}
    public bool getIsSliding {get{return isWallSliding;}}
    public Rigidbody2D getRgb {get{return rgb;}}
    public bool rightSide = true;
    private int direction = 1;
    private bool afterWallJump = false;

    void Awake(){
        input = new InputActionsControl();
    }

    void OnEnable(){
        i_movement = input.PlayerMap.Movement;
        i_movement.Enable();
        input.PlayerMap.Jump.started += JumpStarted;
        input.PlayerMap.Jump.canceled += JumpCanceled;
        input.PlayerMap.Jump.Enable();
    }

    #region Input Section

    void JumpStarted(InputAction.CallbackContext context){
        isPressingJump = true;
        
        if(coyoteTimeCounter > 0f){
            Jump();
        }

        if(GameProgress.Singleton.WallJump){
            WallJump();
        }
        //Double After Wall
        if(!GameProgress.Singleton.DoubleJump){ 
            DoubleJump();
        }
        
    }

    void JumpCanceled(InputAction.CallbackContext context){
        isPressingJump = false;
        
        isJumping = false;
        coyoteTimeCounter = 0f;
    }

    bool InputIsJumping(){
        return isPressingJump;
    }

    #endregion

    void Start(){
        rgb = GetComponent<Rigidbody2D>();
    }

    void Update(){
        //If isPause is true
        //

        //If isPause is false
        if(CheckPause()){ return; }
        
        CheckMovement();
        CheckJump();
        WallSlide();
        Glide();
        GravityUpdate();

    }

    void CheckMovement(){
        if(!isWallJumping){
            movement.x = input.PlayerMap.Movement.ReadValue<Vector2>().x;
        }
        direction = rightSide ? 1 : -1;

        rightSide = movement.x > 0 ? true : movement.x < 0 ? false : rightSide;
        direction = movement.x > 0 ? 1 : movement.x < 0 ? -1 : direction;
    }

    void CheckJump(){
        Vector3 feetPosition = new Vector3(transform.position.x - checkGroundLessX, transform.position.y - checkGroundLessY, transform.position.z);

        isGrounded = Physics2D.OverlapCircle(feetPosition, checkGroundRadius, groundLayer);

        if(isGrounded){
            coyoteTimeCounter = coyoteTime;
        }
        else{
            coyoteTimeCounter -= Time.deltaTime;
        }

        if(isGrounded && jumpCounter > 0){jumpCounter = 0;}

        if(InputIsJumping()){
            if(isJumping == true){
                if(jumpTimeCounter > 0){
                    rgb.velocity = Vector2.up * playerJump;
                    jumpTimeCounter -= Time.deltaTime;
                }
                else{
                    isJumping = false;
                }
            }
        }
    }

    void Jump(){
        jumpCounter++;
        isJumping = true;
        jumpTimeCounter = jumpTime;
        rgb.velocity = Vector2.up * playerJump;
        Interactions.Singleton.Jump();
    }

    void GravityUpdate(){

        if(isGrounded || isWallSliding || isWalled || isWallJumping){
            rgb.gravityScale = 1.5f;
        }
        else if(!isJumping){
            rgb.gravityScale = 2.5f;
        }

    }

    void WallSlide(){
        if(!GameProgress.Singleton.WallJump){ return; }

        Vector3 wallPosition = new Vector3(transform.position.x + (checkWallMoreX * direction), transform.position.y, transform.position.z);
        isWalled = Physics2D.OverlapCircle(wallPosition, checkGroundRadius, groundLayer);

        if(isWalled && !isGrounded && movement.x != 0){
            isWallSliding = true;
            rgb.velocity = new Vector2(rgb.velocity.x, Mathf.Clamp(rgb.velocity.y, -playerSlider, float.MaxValue));
        }
        else{
            isWallSliding = false;
        }

        if(isWallSliding){
            wallJumpingDirection = rightSide ? -1 : 1;
            wallJumpingCounter = wallJumpingTime;
        }
        else{
            if(wallJumpingCounter > 0f){
                wallJumpingCounter -= Time.deltaTime;
            }
        }
    }

    void WallJump(){
        if(wallJumpingCounter > 0f){
            rgb.velocity = Vector2.zero;
            movement.x = wallJumpingDirection;

            isWallJumping = true;
            Invoke(nameof(ResetWallJumping), 0.2f);

            rgb.velocity = Vector2.zero;
            rgb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            rightSide = !rightSide;

            jumpCounter++;
            Interactions.Singleton.Jump();
            StartCoroutine(ActiveAfterWallJump());
        }
    }

    void Glide(){
        if(!InputIsJumping() || !GameProgress.Singleton.Glide){ return; }

        if(!isGrounded && !isWallSliding){
            rgb.velocity = new Vector2(rgb.velocity.x, Mathf.Clamp(rgb.velocity.y, -playerGlider, float.MaxValue));
        }
    }

    void ResetWallJumping(){
        isWallJumping = false;
    }

    void DoubleJump(){        
        if(!isGrounded && jumpCounter < maxJumps && !isWallSliding && !afterWallJump){
            Jump();
            //Debug.Log($"DoubleJump isWallSliding {isWallSliding} isWalled {isWalled}");
        }
    }

    void FixedUpdate(){

        if(CheckPause()){ return; }
        
        if(!isWallJumping){
            rgb.velocity = new Vector2(movement.x * playerVelocity * Time.fixedDeltaTime, rgb.velocity.y);
        }
    }

    IEnumerator ActiveAfterWallJump(){
        afterWallJump = true;
        yield return new WaitForSeconds(0.15f);
        afterWallJump = false;
    }

    bool CheckPause(){
        if(PauseSystem.Singleton.CheckIsPause){
            return true;
        }
        else if(DialogCommands.IsDialogActive()){
            if(movement != Vector2.zero){
                rgb.velocity = Vector2.zero;
                movement = Vector2.zero;
            }
            return true;
        }
        return false;
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.yellow;
        Vector3 feetPosition = new Vector3(transform.position.x - checkGroundLessX, transform.position.y - checkGroundLessY, transform.position.z);
        Gizmos.DrawSphere(feetPosition, checkGroundRadius);

        Vector3 wallPosition = new Vector3(transform.position.x + (checkWallMoreX * direction), transform.position.y, transform.position.z);
        Gizmos.DrawSphere(wallPosition, checkWallRadius);
    }



}
