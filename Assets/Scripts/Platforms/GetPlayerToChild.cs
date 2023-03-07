using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerToChild : MonoBehaviour
{
    private GameObject child;
    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("PlayerTrigger") && IsAbove(col)){
            child = GameObject.FindGameObjectWithTag("Player").gameObject;
            child.transform.parent = transform.root;
        }
    }

    void OnTriggerExit2D(Collider2D col){
        if(col.CompareTag("PlayerTrigger")){
            if(child != null){
                child.transform.parent = null;
                child = null;
            }
        }
    }

    bool IsAbove(Collider2D col){
        Vector2 bottomPoint = new Vector2(col.bounds.center.x, col.bounds.center.y - col.bounds.extents.y);
        bottomPoint.y += 0.02f;
        return bottomPoint.y > transform.position.y;
    }

    void Update(){
        if(child != null && GetComponent<Collider2D>().enabled == false){
            child.transform.parent = null;
        }
    }

    public void CheckChildToTransformPosition(){
        if(child != null){
            child.transform.parent = null;
            child = null;
        }
    }
}
