using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCollider : MonoBehaviour
{
    [SerializeField] private PlatformUnstable platformUnstable;

    [EasyButtons.Button]
    void Test(){
        CallFunction();
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("PlayerTrigger")){
            CallFunction();
        }
    }

    void CallFunction(){
        platformUnstable?.Break();
    }
}
