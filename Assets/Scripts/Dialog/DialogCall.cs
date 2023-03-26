using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleDialogN;
using UnityEngine.InputSystem;

public class DialogCall : MonoBehaviour
{
    private InputActionsControl input;

    private bool isColliding = false;
    private DialogUnit dialogUnit;
    [SerializeField] private SO_Dialog so_Dialog;

    [SerializeField] private SpriteRenderer sprite;
    private bool notificationBool = false;

    void Awake()
    {
        input = new InputActionsControl();
    }

    void OnEnable()
    {
        input.PlayerMap.Interact.started += InteractButton;
        input.PlayerMap.Interact.Enable();
    }

    void InteractButton(InputAction.CallbackContext context)
    {
        if (DialogCommands.IsDialogActive()){
            DialogCommands.PlayDialog();
        }
        else if (dialogUnit != null){

            try{
                List<string> dialogToCall = so_Dialog.mainDialogs.Find(x => x.id == dialogUnit.id).dialog;
                DialogCommands.CallDialog(dialogToCall);
            }
            catch{
                Debug.LogError("There's no valid dialog in the current Id");
            }

        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<DialogUnit>() != null)
        {
            dialogUnit = col.GetComponent<DialogUnit>();
            UpdateSprite();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<DialogUnit>() != null)
        {
            dialogUnit = null;
            UpdateSprite();
        }
    }

    void UpdateSprite()
    {
        notificationBool = dialogUnit != null;
        if ((sprite.enabled && !notificationBool) || (!sprite.enabled && notificationBool))
        {
            sprite.enabled = notificationBool;
        }
    }
}
