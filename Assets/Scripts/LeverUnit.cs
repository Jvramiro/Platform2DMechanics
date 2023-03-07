using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverUnit : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public int id;
    public void SwitchLever(){
        anim.Play("LeverActive");
        anim.SetFloat("Actived", anim.GetFloat("Actived") == 0 ? 1 : 0);
    }
}
