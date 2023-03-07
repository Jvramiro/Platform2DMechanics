using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Interactions interactions;
    private float checkxAxys;

    void OnEnable() => interactions.Jump += Jump;
    void OnDisable() => interactions.Jump -= Jump;

    void Update(){
        if(checkxAxys != playerMovement.getMovement.x){
            checkxAxys = playerMovement.getMovement.x;
            anim.SetFloat("xAxys", Mathf.Abs(checkxAxys));
            sprite.flipX = checkxAxys < 0 ? true : checkxAxys > 0 ? false : sprite.flipX;
        }
        anim.SetBool("isGrounded", playerMovement.getIsGrounded);
        anim.SetBool("isSliding", playerMovement.getIsSliding);
    }

    void Jump(){
        anim.Play("PlayerJumping");
    }
}
