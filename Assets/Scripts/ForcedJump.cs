using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CollisionExtensions;
using UnityEngine.InputSystem;

public class ForcedJump : MonoBehaviour
{
    private InputActionsControl input;
    private bool i_isJumping;

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Collider2D col;
    [SerializeField] private float jumpForce = 180, pressingMultiplier = 1.5f;
    private bool isColliding = false, toReset = false;
    private float jumpTimeCounter;
    [SerializeField] private float playerJump = 2.5f, jumpTime = 0.3f;

    void Awake(){
        input = new InputActionsControl();
    }

    void OnEnable(){
        input.PlayerMap.Jump.started += JumpStarted;
        input.PlayerMap.Jump.canceled += JumpCanceled;
        input.PlayerMap.Jump.Enable();
    }

    #region Input Section

    void JumpStarted(InputAction.CallbackContext context){
        i_isJumping = true;
        JumpToForce();        
    }

    void JumpCanceled(InputAction.CallbackContext context){
        i_isJumping = false;
    }

    bool InputIsJumping(){
        return i_isJumping;
    }

    #endregion

    void OnTriggerEnter2D(Collider2D col){

        if(col.CompareTag("PlayerTrigger") && CollisionExtension.IsAbove(col, transform.position)){
            isColliding = true;
            if(!toReset){
                PlayerForcedJump();
            }
        }

    }

    void PlayerForcedJump(){

        toReset = true;
        jumpTimeCounter = jumpTime;

        Rigidbody2D rgb = GetPlayerMovement().getRgb;
        rgb.velocity = Vector2.zero;
        float multiplier = !InputIsJumping() ? 1 : pressingMultiplier;
        rgb.AddForce(Vector2.up * jumpForce * multiplier);

        CancelInvoke(nameof(ResetForcedJump));
        Invoke(nameof(ResetForcedJump),0.1f);

        CancelInvoke(nameof(BreakObject));
        Invoke(nameof(BreakObject),0.2f);
    }

    void ResetForcedJump(){
        toReset = false;
    }

    void BreakObject(){
        sprite.enabled = false;
        col.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        CancelInvoke(nameof(ResetObject));
        Invoke(nameof(ResetObject),1f);
    }

    void ResetObject(){
        sprite.enabled = true;
        col.enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    PlayerMovement GetPlayerMovement(){
        return FindObjectOfType<PlayerMovement>();
    }

    void JumpToForce(){
        if(toReset == true){
            if(jumpTimeCounter > 0){
                Rigidbody2D rgb = GetPlayerMovement().getRgb;
                rgb.velocity = Vector2.up * playerJump;
                jumpTimeCounter -= Time.deltaTime;
            }
        }
    }
}
