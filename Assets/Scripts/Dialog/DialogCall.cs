using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleDialogN;

public class DialogCall : MonoBehaviour
{
    private bool isColliding = false;
    private DialogUnit dialogUnit;
    [SerializeField] private SO_Dialog so_Dialog;

    [SerializeField] private SpriteRenderer sprite;
    private bool notificationBool = false;

    void Update(){

        if(Input.GetKeyDown(KeyCode.C)){
            if(DialogCommands.IsDialogActive()){
                DialogCommands.PlayDialog();
            }
            else if(dialogUnit != null){

                try{
                    List<string> dialogToCall = so_Dialog.mainDialogs.Find(x => x.id == dialogUnit.id).dialog;
                    DialogCommands.CallDialog(dialogToCall);
                }
                catch{
                    Debug.LogError("There's no valid dialog in the current Id");
                }

            }

        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.GetComponent<DialogUnit>() != null){
            dialogUnit = col.GetComponent<DialogUnit>();
            UpdateSprite();
        }
    }

    void OnTriggerExit2D(Collider2D col){
        if(col.GetComponent<DialogUnit>() != null){
            dialogUnit = null;
            UpdateSprite();
        }
    }

    void UpdateSprite(){
        notificationBool = dialogUnit != null;
        if( (sprite.enabled && !notificationBool) || (!sprite.enabled && notificationBool) ){
            sprite.enabled = notificationBool;
        }
    }
}
