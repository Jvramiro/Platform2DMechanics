using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleDialogN;

public class PlayerMovement : MonoBehaviour
{
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
        WallJump();
        //After Wall Jump because boolean afterWallJump
        DoubleJump();

        Glide();

    }

    void CheckMovement(){
        if(!isWallJumping){
            movement.x = Input.GetAxisRaw("Horizontal");
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

        if(coyoteTimeCounter > 0f && Input.GetKeyDown(KeyCode.X)){
            Jump();
        }
        if(Input.GetKey(KeyCode.X) && isJumping == true){
            if(jumpTimeCounter > 0){
                rgb.velocity = Vector2.up * playerJump;
                jumpTimeCounter -= Time.deltaTime;
            }
            else{
                isJumping = false;
            }
        }
        if(Input.GetKeyUp(KeyCode.X)){
            isJumping = false;
            coyoteTimeCounter = 0f;
        }

        if(isGrounded && jumpCounter > 0){jumpCounter = 0;}
    }

    void Jump(){
        jumpCounter++;
        isJumping = true;
        jumpTimeCounter = jumpTime;
        rgb.velocity = Vector2.up * playerJump;
        Interactions.Singleton.Jump();
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
    }

    void WallJump(){
        if(!GameProgress.Singleton.WallJump){ return; }

        if(isWallSliding){
            wallJumpingDirection = rightSide ? -1 : 1;
            wallJumpingCounter = wallJumpingTime;
        }
        else{
            if(wallJumpingCounter > 0f){
                wallJumpingCounter -= Time.deltaTime;
            }
        }
        if(Input.GetKeyDown(KeyCode.X) && wallJumpingCounter > 0f){
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

            //Debug.Log("WallJump");
        }
    }

    void Glide(){
        if(!GameProgress.Singleton.Glide){ return; }

        if(Input.GetKey(KeyCode.X) && !isGrounded && !isWallSliding){
            rgb.velocity = new Vector2(rgb.velocity.x, Mathf.Clamp(rgb.velocity.y, -playerGlider, float.MaxValue));
        }
    }

    void ResetWallJumping(){
        isWallJumping = false;
    }

    void DoubleJump(){
        if(!GameProgress.Singleton.DoubleJump){ return; }
        
        if(Input.GetKeyDown(KeyCode.X) && !isGrounded && jumpCounter < maxJumps && !isWallSliding && !afterWallJump){
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
